using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChannelFactory<IWeatherStationService> factory = new ChannelFactory<IWeatherStationService>("Primary");
                IWeatherStationService channel = factory.CreateChannel();


                List<CurrentWeatherData> data = channel.GetInRange(DateTime.Now.Date.AddDays(1).AddHours(8), DateTime.Now.Date.AddDays(3).AddHours(20), "Novi Sad");
                foreach(CurrentWeatherData dataItem in data)
                {
                    Console.WriteLine(dataItem);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("FAILED TO CONNECT TO SERVICE 1: " + ex.Message);
            }

            Console.WriteLine("Press[Enter] to stop the client.");
            Console.ReadLine();


        }
    }
}
