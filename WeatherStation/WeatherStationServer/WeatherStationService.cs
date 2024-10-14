using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace WeatherStationServer
{
    public class WeatherStationService : IWeatherStationService
    {
        static List<CurrentWeatherData> storedData = new List<CurrentWeatherData>();

        public void Add(CurrentWeatherData data)
        {
            CurrentWeatherData existingData = storedData.FirstOrDefault(d => d.Location.ToLower().Equals(data.Location.ToLower())
                                                                          && d.Timestamp.Date == data.Timestamp.Date                                                            && d.Timestamp.Hour == data.Timestamp.Hour);
            if(existingData != null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA ALREADY EXISTS!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }

            storedData.Add(data);
            Console.WriteLine("1 NEW DATA ADDED: " + data);
        }

        public void Update(CurrentWeatherData data)
        {
            CurrentWeatherData existingData = storedData.FirstOrDefault(d => d.Location.ToLower().Equals(data.Location.ToLower())
                                                                     && d.Timestamp.Date == data.Timestamp.Date && d.Timestamp.Hour == data.Timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }

            existingData.Update(data);
            Console.WriteLine("1 DATA MODIFIED: " + data);
        }

        public CurrentWeatherData Get(DateTime timestamp, string location)
        {
            throw new NotImplementedException();
        }

        public double GetAverageHumidity(DateTime day, string location)
        {
            throw new NotImplementedException();
        }

        public double GetAverageTemperature(DateTime day, string location)
        {
            throw new NotImplementedException();
        }

        public int GetClearDaysNumber(DateTime month, string location)
        {
            throw new NotImplementedException();
        }

        public List<int> GetExtremeUVIndexHours(DateTime day, string location)
        {
            throw new NotImplementedException();
        }

        public double GetHumidity(DateTime timestamp, string location)
        {
            throw new NotImplementedException();
        }

        public List<CurrentWeatherData> GetInRange(DateTime from, DateTime to, string location)
        {
            throw new NotImplementedException();
        }

        public (double, double) GetMinMaxPrecitipation(DateTime day, string location)
        {
            throw new NotImplementedException();
        }

        public (double, double) GetMinMaxTemperature(DateTime day, string location)
        {
            throw new NotImplementedException();
        }

        public double GetPrecipitation(DateTime timestamp, string location)
        {
            throw new NotImplementedException();
        }

        public double GetPressure(DateTime timestamp, string location)
        {
            throw new NotImplementedException();
        }

        public int GetRainyDaysNumber(DateTime month, string location)
        {
            throw new NotImplementedException();
        }

        public double GetTemperature(DateTime timestamp, string location)
        {
            throw new NotImplementedException();
        }

        public double GetUVIndex(DateTime timestamp, string location)
        {
            throw new NotImplementedException();
        }

        public WindDirection GetWindDirection(DateTime timestamp, string location)
        {
            throw new NotImplementedException();
        }

        public double GetWindSpeed(DateTime timestamp, string location)
        {
            throw new NotImplementedException();
        }
    }
}
