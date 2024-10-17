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
        static IServerStateService channelPrimary = null;
        static IServerStateService channelSecondary = null;
        static IWeatherStationService channelSource = null;
        static IWeatherStationService channelDestination = null;
        static EServerState primaryState = EServerState.Unknown;
        static EServerState secondaryState = EServerState.Unknown;
        static void Main(string[] args)
        {
            DateTime lastReplicationTime = DateTime.MinValue;
            

            while (true)
            {
                try
                {
                    ChannelFactory<IWeatherStationService> cfSource = new ChannelFactory<IWeatherStationService>("Source");
                    ChannelFactory<IWeatherStationService> cfDestination = new ChannelFactory<IWeatherStationService>("Destination");
                    channelSource = cfSource.CreateChannel();
                    channelDestination = cfDestination.CreateChannel();

                    ChannelFactory<IServerStateService> cfPrimary = new ChannelFactory<IServerStateService>("Primary");
                    ChannelFactory<IServerStateService> cfSecondary = new ChannelFactory<IServerStateService>("Secondary");
                    channelPrimary = cfPrimary.CreateChannel();
                    channelSecondary = cfSecondary.CreateChannel();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


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

                try
                {
                    if(primaryState == EServerState.Primary && secondaryState != EServerState.Primary)
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
}
