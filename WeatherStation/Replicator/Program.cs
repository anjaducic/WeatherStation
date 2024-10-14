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
        static void Main(string[] args)
        {
            DateTime lastReplicationTime = DateTime.MinValue;
            

            while (true)
            {
                try
                {
                    ChannelFactory<IWeatherStationService> cfSource = new ChannelFactory<IWeatherStationService>("Source");
                    ChannelFactory<IWeatherStationService> cfDestination = new ChannelFactory<IWeatherStationService>("Destination");
                    IWeatherStationService channelSource = cfSource.CreateChannel();
                    IWeatherStationService channelDestination = cfDestination.CreateChannel();
                    channelDestination.EnterData(channelSource.GetSince(lastReplicationTime));
                    channelDestination.GetSince(lastReplicationTime);    //samo da testiram
                    lastReplicationTime = DateTime.Now;

                    Thread.Sleep(3000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }
    }
}
