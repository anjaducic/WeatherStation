using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using SharedLibrary;


namespace DesktopClient
{
    public class ServiceManager
    {
        static private ChannelFactory<IWeatherStationService> factorySource;
        static private ChannelFactory<IWeatherStationService> factoryDestination;
        static private ChannelFactory<IServerStateService> factoryPrimary;
        static private ChannelFactory<IServerStateService> factorySecondary;
        static private IWeatherStationService channelSource = null;
        static private IWeatherStationService channelDestination = null;
        static private IServerStateService channelPrimary = null;
        static private IServerStateService channelSecondary = null;
        static private EServerState primaryState = EServerState.Unknown;
        static private EServerState secondaryState = EServerState.Unknown;

        public ServiceManager(){}

        public static void ConnectToService()
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
                    secondaryState = EServerState.Unknown;
                }
            }

            
        }



        public static bool AddNewData(CurrentWeatherData newWeatherData)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                channel.Add(newWeatherData);
                return true;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }


        public static CurrentWeatherData GetOneData(string location, DateTime timestamp)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                CurrentWeatherData weatherData = channel.Get(timestamp, location);
                return weatherData;
                
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static List<CurrentWeatherData> GetDataInRange(DateTime from, DateTime to, string location)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                List<CurrentWeatherData> weatherDataList = channel.GetInRange(from, to, location);
                return weatherDataList;
               
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static double? GetCurrentTemperature(string location, DateTime timestamp)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                double currentTemperature = channel.GetTemperature(timestamp, location);
                return currentTemperature;

            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static double? GetCurrentPressure(string location, DateTime timestamp)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                double currentPressure = channel.GetPressure(timestamp, location);
                return currentPressure;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static double? GetWindSpeed(string location, DateTime timestamp)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                double currentWindSpeed = channel.GetWindSpeed(timestamp, location);
                return currentWindSpeed;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static WindDirection? GetWindDirection(string location, DateTime timestamp)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                WindDirection currentWindDirection = channel.GetWindDirection(timestamp, location);
                return currentWindDirection;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static double? GetPrecipitation(string location, DateTime timestamp)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                double currentPrecipitation = channel.GetPrecipitation(timestamp, location);
                return currentPrecipitation;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static double? GetHumidity(string location, DateTime timestamp)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                double currentHumidity = channel.GetHumidity(timestamp, location);
                return currentHumidity;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static double? GetUVIndex(string location, DateTime timestamp)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                double currentUVIndex = channel.GetUVIndex(timestamp, location);
                return currentUVIndex;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static double? GetAverageTemperature(string location, DateTime day)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                double averageTemperature = channel.GetAverageTemperature(day, location);
                return averageTemperature;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static (double, double)? GetMinMaxTemperature(string location, DateTime day)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                var (minTemp, maxTemp) = channel.GetMinMaxTemperature(day, location);
                return (minTemp, maxTemp);
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static int? GetClearDaysNumber(string location, DateTime month)
        {
            try
            {
               ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                int clearDays = channel.GetClearDaysNumber(month, location);
                return clearDays;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static int? GetRainyDaysNumber(string location, DateTime month)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                int rainyDays = channel.GetRainyDaysNumber(month, location);
                return rainyDays;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static (double, double)? GetMinMaxPrecipitation(string location, DateTime day)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                var (minPrecipitation, maxPrecipitation) = channel.GetMinMaxPrecitipation(day, location);
                return (minPrecipitation, maxPrecipitation);
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static double? GetAverageHumidity(string location, DateTime day)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                double averageHumidity = channel.GetAverageHumidity(day, location);
                return averageHumidity;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        public static List<int> GetExtremeUVIndexHours(string location, DateTime day)
        {
            try
            {
                ConnectToService();
                IWeatherStationService channel = null;
                if (primaryState == EServerState.Primary)
                    channel = channelSource;
                else if (secondaryState == EServerState.Primary)
                    channel = channelDestination;
                else
                {
                    MessageBox.Show("SERVER ERROR. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                List<int> extremeUVHours = channel.GetExtremeUVIndexHours(day, location);
                return extremeUVHours;
            }
            catch (FaultException<WeatherDataServiceException> ex)
            {
                MessageBox.Show(ex.Detail.Reason, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }
    }
}
