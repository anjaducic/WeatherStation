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
        static ChannelFactory<IWeatherStationService> factorySource;
        static ChannelFactory<IWeatherStationService> factoryDestination;
        static ChannelFactory<IServerStateService> factoryPrimary;
        static ChannelFactory<IServerStateService> factorySecondary;
        static IWeatherStationService channelSource = null;
        static IWeatherStationService channelDestination = null;
        static IServerStateService channelPrimary = null;
        static IServerStateService channelSecondary = null;
        static EServerState primaryState = EServerState.Unknown;
        static EServerState secondaryState = EServerState.Unknown;

        static void Main(string[] args)
        {
            while (true) 
            {
                ShowMenu();
                string choice = Console.ReadLine();
                switch (choice.ToUpper())
                {
                    case "A":
                        AddNewData();
                        break;
                    case "B":
                        UpdateData();
                        break;
                    case "C":
                        GetOneData();
                        break;
                    case "D":
                        GetDataInRange();
                        break;
                    case "E":
                        GetCurrentTemperature();
                        break;
                    case "F":
                        GetCurrentPressure();
                        break;
                    case "G":
                        GetWindSpeed();
                        break;
                    case "H":
                        GetWindDirection();
                        break;
                    case "I":
                        GetPrecipitation();
                        break;
                    case "J":
                        GetHumidity();
                        break;
                    case "K":
                        GetUVIndex();
                        break;
                    case "L":
                        GetAverageTemperature();
                        break;
                    case "M":
                        GetMinMaxTemperature();
                        break;
                    case "N":
                        GetClearDaysNumber();
                        break;
                    case "O":
                        GetRainyDaysNumber();
                        break;
                    case "P":
                        GetMinMaxPrecipitation();
                        break;
                    case "Q":
                        GetAverageHumidity();
                        break;
                    case "R":
                        GetExtremeUVIndexHours();
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }               
            }
      
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\nOPTIONS:\n");
            Console.WriteLine("1. Press A to add new data.\n" +
                              "2. Press B to update data.\n" +
                              "3. Press C to get one data.\n" +
                              "4. Press D to get data in time range.\n" +
                              "5. Press E to get current temperature.\n" +
                              "6. Press F to get current pressure.\n" +
                              "7. Press G to get wind speed.\n" +
                              "8. Press H to get wind direction.\n" +
                              "9. Press I to get precipitation.\n" +
                              "10. Press J to get humidity.\n" +
                              "11. Press K to get UV index.\n" +
                              "12. Press L to get average daily temperature.\n" +
                              "13. Press M to get min and max daily temperature.\n" +
                              "14. Press N to get clear days number in month.\n" +
                              "15. Press O to get rainy days number in month.\n" +
                              "16. Press P to get min and max daily precipiration.\n" +
                              "17. Press Q to get average daily humidity.\n" +
                              "18. Press R to get extreme UV index hours.\n" +
                              "19. Press 0 to exit.\n" +
                              "\n Enter option: ");
        }

        private static void ConnectToService()
        {
            try
            {
                factorySource = new ChannelFactory<IWeatherStationService>("Source");
                channelSource = factorySource.CreateChannel();
                factoryPrimary = new ChannelFactory<IServerStateService>("Primary");
                channelPrimary = factoryPrimary.CreateChannel();
                primaryState = channelPrimary.GetState();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                primaryState = EServerState.Unknown;
            }

            if (primaryState != EServerState.Primary)
            {
                try
                {
                    factoryDestination = new ChannelFactory<IWeatherStationService>("Destination");
                    channelDestination = factoryDestination.CreateChannel();
                    factorySecondary = new ChannelFactory<IServerStateService>("Secondary");
                    channelSecondary = factorySecondary.CreateChannel();
                    secondaryState = channelSecondary.GetState();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    secondaryState = EServerState.Unknown;
                }
            }
        }

        private static void AddNewData()
        {
            try
            {
                Console.WriteLine("ENTER NEW DATA:\n");
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

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                channel.Add(newWeatherData);
                Console.WriteLine("1 NEW DATA ADDED.\n");

            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine(ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void UpdateData()
        {  

            try
            {
                Console.WriteLine("ENTER UPDATED DATA:\n");

                Console.WriteLine("Enter location of the data you want to update:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp of the data you want to update (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                Console.WriteLine("Enter new temperature (°C):");
                double temperature = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter new pressure (hPa):");
                double pressure = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter new wind speed (km/h):");
                double windSpeed = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter new wind direction (North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest):");
                WindDirection windDirection;
                while (!Enum.TryParse(Console.ReadLine(), out windDirection) || !Enum.IsDefined(typeof(WindDirection), windDirection))
                {
                    Console.WriteLine("Invalid wind direction. Please enter a valid wind direction (North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest):");
                }

                Console.WriteLine("Enter new precipitation (mm):");
                double precipitation = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter new UV Index:");
                double uvIndex = Double.Parse(Console.ReadLine());

                Console.WriteLine("Enter new humidity (%):");
                double humidity = Double.Parse(Console.ReadLine());

                CurrentWeatherData updatedWeatherData = new CurrentWeatherData(location, timestamp, temperature, pressure, windSpeed, windDirection, precipitation, uvIndex, humidity);

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }


                channel.Update(updatedWeatherData);
                Console.WriteLine("DATA UPDATED SUCCESSFULLY.\n");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine(ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetOneData()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                CurrentWeatherData weatherData = channel.Get(timestamp, location);

                Console.WriteLine("Weather Data Found:\n");
                Console.WriteLine(weatherData.ToString());
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetDataInRange()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter start date (yyyy-mm-dd HH:mm:ss):");
                DateTime from;
                while (!DateTime.TryParse(Console.ReadLine(), out from))
                {
                    Console.WriteLine("Invalid format. Please enter the start date in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                Console.WriteLine("Enter end date (yyyy-mm-dd HH:mm:ss):");
                DateTime to;
                while (!DateTime.TryParse(Console.ReadLine(), out to))
                {
                    Console.WriteLine("Invalid format. Please enter the end date in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                List<CurrentWeatherData> weatherDataList = channel.GetInRange(from, to, location);

                Console.WriteLine("Filtered data in the range:\n");
                foreach (var data in weatherDataList)
                {
                    Console.WriteLine(data);
                }
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetCurrentTemperature()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                double currentTemperature = channel.GetTemperature(timestamp, location);

                Console.WriteLine("Current temperature in {location} at {timestamp:yyyy-MM-dd HH:mm:ss} is {currentTemperature}°C");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetCurrentPressure()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                double currentPressure = channel.GetPressure(timestamp, location);

                Console.WriteLine("Current pressure in {location} at {timestamp:yyyy-MM-dd HH:mm:ss} is {currentPressure} hPa");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetWindSpeed()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                double currentWindSpeed = channel.GetWindSpeed(timestamp, location);

                Console.WriteLine("Current wind speed in {location} at {timestamp:yyyy-MM-dd HH:mm:ss} is {currentWindSpeed} km/h");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetWindDirection()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                WindDirection currentWindDirection = channel.GetWindDirection(timestamp, location);

                Console.WriteLine("Current wind direction in {location} at {timestamp:yyyy-MM-dd HH:mm:ss} is {currentWindDirection}");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetPrecipitation()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                double currentPrecipitation = channel.GetPrecipitation(timestamp, location);

                Console.WriteLine("Current precipitation in {location} at {timestamp:yyyy-MM-dd HH:mm:ss} is {currentPrecipitation} mm");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetHumidity()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                double currentHumidity = channel.GetHumidity(timestamp, location);

                Console.WriteLine("Current humidity in {location} at {timestamp:yyyy-MM-dd HH:mm:ss} is {currentHumidity}%");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetUVIndex()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter timestamp (yyyy-mm-dd HH:mm:ss):");
                DateTime timestamp;
                while (!DateTime.TryParse(Console.ReadLine(), out timestamp))
                {
                    Console.WriteLine("Invalid format. Please enter the timestamp in the correct format (yyyy-mm-dd HH:mm:ss):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                double currentUVIndex = channel.GetUVIndex(timestamp, location);

                Console.WriteLine($"Current UV index in {location} at {timestamp:yyyy-MM-dd HH:mm:ss} is {currentUVIndex}");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetAverageTemperature()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter date (yyyy-mm-dd):");
                DateTime day;
                while (!DateTime.TryParse(Console.ReadLine(), out day))
                {
                    Console.WriteLine("Invalid format. Please enter the date in the correct format (yyyy-mm-dd):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                double averageTemperature = channel.GetAverageTemperature(day, location);

                Console.WriteLine("Average daily temperature in {location} on {day:yyyy-MM-dd} is {averageTemperature} °C");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetMinMaxTemperature()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter date (yyyy-mm-dd):");
                DateTime day;
                while (!DateTime.TryParse(Console.ReadLine(), out day))
                {
                    Console.WriteLine("Invalid format. Please enter the date in the correct format (yyyy-mm-dd):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                var (minTemp, maxTemp) = channel.GetMinMaxTemperature(day, location);

                Console.WriteLine("Min and Max daily temperature in {location} on {day:yyyy-MM-dd} is {minTemp} °C and {maxTemp} °C");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetClearDaysNumber()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter month (yyyy-mm):");
                DateTime month;
                while (!DateTime.TryParse(Console.ReadLine() + "-01", out month))
                {
                    Console.WriteLine("Invalid format. Please enter the month in the correct format (yyyy-mm):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                int clearDays = channel.GetClearDaysNumber(month, location);

                Console.WriteLine("Number of clear days in {location} for {month:yyyy-MM} is {clearDays} days");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetRainyDaysNumber()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter month (yyyy-mm):");
                DateTime month;
                while (!DateTime.TryParse(Console.ReadLine() + "-01", out month))
                {
                    Console.WriteLine("Invalid format. Please enter the month in the correct format (yyyy-mm):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                int rainyDays = channel.GetRainyDaysNumber(month, location);

                Console.WriteLine("Number of rainy days in {location} for {month:yyyy-MM} is {rainyDays} days");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetMinMaxPrecipitation()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter date (yyyy-mm-dd):");
                DateTime day;
                while (!DateTime.TryParse(Console.ReadLine(), out day))
                {
                    Console.WriteLine("Invalid format. Please enter the date in the correct format (yyyy-mm-dd):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                var (minPrecipitation, maxPrecipitation) = channel.GetMinMaxPrecitipation(day, location);

                Console.WriteLine("Min and Max daily precipitation in {location} on {day:yyyy-MM-dd} is {minPrecipitation} mm and {maxPrecipitation} mm");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetAverageHumidity()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter date (yyyy-mm-dd):");
                DateTime day;
                while (!DateTime.TryParse(Console.ReadLine(), out day))
                {
                    Console.WriteLine("Invalid format. Please enter the date in the correct format (yyyy-mm-dd):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                double averageHumidity = channel.GetAverageHumidity(day, location);

                Console.WriteLine("Average daily humidity in {location} on {day:yyyy-MM-dd} is {averageHumidity} %");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


        private static void GetExtremeUVIndexHours()
        {
            try
            {
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();

                Console.WriteLine("Enter date (yyyy-mm-dd):");
                DateTime day;
                while (!DateTime.TryParse(Console.ReadLine(), out day))
                {
                    Console.WriteLine("Invalid format. Please enter the date in the correct format (yyyy-mm-dd):");
                }

                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    Console.WriteLine("Services not available. Try again...");
                    return;
                }

                List<int> extremeUVHours = channel.GetExtremeUVIndexHours(day, location);

                Console.WriteLine("Extreme UV Index hours in {location} on {day:yyyy-MM-dd}: {string.Join(", ", extremeUVHours)}");
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                Console.WriteLine("Error: " + ex.Detail.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }


    }
}
