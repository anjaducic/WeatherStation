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

                //1
                List<CurrentWeatherData> data = channel.GetInRange(DateTime.Now.Date.AddDays(1).AddHours(8), DateTime.Now.Date.AddDays(3).AddHours(20), "Novi Sad");
                foreach(CurrentWeatherData dataItem in data)
                {
                    Console.WriteLine(dataItem);
                }

                //2
                Console.WriteLine("NEW DATA:\n");
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                Console.WriteLine("Enter temperature (°C):");
                double temperature = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter pressure (hPa):");
                double pressure = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter wind speed (km/h):");
                double windSpeed = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter wind direction (North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest):");
                WindDirection windDirection;
                while (!Enum.TryParse(Console.ReadLine(), out windDirection) || !Enum.IsDefined(typeof(WindDirection), windDirection))
                {
                    Console.WriteLine("Invalid wind direction. Please enter a valid wind direction (North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest):");
                }

                Console.WriteLine("Enter precipitation (mm):");
                double precipitation = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter UV Index:");
                double uvIndex = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter humidity (%):");
                double humidity = Double.Parse(Console.ReadLine());

                CurrentWeatherData newWeatherData = new CurrentWeatherData(location, timestamp, temperature, pressure, windSpeed, windDirection, precipitation, uvIndex, humidity);
                channel.Add(newWeatherData);
                Console.WriteLine("1 NEW DATA ADDED.\n");



            }
            catch (Exception ex)
            {
                Console.WriteLine("FAILED TO CONNECT TO SERVICE 1: " + ex.Message);
            }

            Console.WriteLine("Press[Enter] to stop the client.");
            Console.ReadLine();


        }
    }
}
