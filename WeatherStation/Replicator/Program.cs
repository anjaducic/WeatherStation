using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedLibrary;

namespace Replicator
{
    public class Program
    {
        static ChannelFactory<IWeatherStationService> factorySource;
        static ChannelFactory<IWeatherStationService> factoryDestination;
        static ChannelFactory<IServerStateService> factoryPrimary;
        static ChannelFactory<IServerStateService> factorySecondary;
        static IServerStateService channelPrimary = null;
        static IServerStateService channelSecondary = null;
        static IWeatherStationService channelSource = null;
        static IWeatherStationService channelDestination = null;
        static EServerState primaryState = EServerState.Unknown;
        static EServerState secondaryState = EServerState.Unknown;
        static DateTime lastReplicationTime = DateTime.MinValue;
        static void Main(string[] args)
        {
            while (true)
            {
                ConnectToServices();
                GetStates();
                Replicate();
            }
        }
        private static void ConnectToServices()
        {
            try
            {
                factorySource = new ChannelFactory<IWeatherStationService>("Source");
                factoryDestination = new ChannelFactory<IWeatherStationService>("Destination");
                channelSource = factorySource.CreateChannel();
                channelDestination = factoryDestination.CreateChannel();

                factoryPrimary = new ChannelFactory<IServerStateService>("Primary");
                factorySecondary = new ChannelFactory<IServerStateService>("Secondary");
                channelPrimary = factoryPrimary.CreateChannel();
                channelSecondary = factorySecondary.CreateChannel();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void GetStates()
        {
            try
            {
                primaryState = channelPrimary.GetState();
            }
            catch
            {
                Console.WriteLine("SERVICE-A NOT AVAILABLE.");
                primaryState = EServerState.Unknown;
            }

            try
            {
                secondaryState = channelSecondary.GetState();
            }
            catch
            {
                Console.WriteLine("SERVICE-B NOT AVAILABLE");
                secondaryState = EServerState.Unknown;
            }
        }

        private static void Replicate()
        {
            try
            {
                if (primaryState == EServerState.Primary && secondaryState != EServerState.Primary)
                {
                    channelDestination.EnterData(channelSource.GetSince(lastReplicationTime));
                    channelDestination.GetSince(lastReplicationTime);    //for testing
                    lastReplicationTime = DateTime.Now;                 //refresh if replication is successful

                    Console.WriteLine("REPLICATION A -> B\n");
                    Thread.Sleep(3000);
                }
                else if (secondaryState == EServerState.Primary && primaryState != EServerState.Primary)
                {
                    channelSource.EnterData(channelDestination.GetSince(lastReplicationTime));
                    channelSource.GetSince(lastReplicationTime);
                    lastReplicationTime = DateTime.Now;

                    Console.WriteLine("REPLICATION B -> A\n");
                    Thread.Sleep(3000);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}
