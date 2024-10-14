using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace WeatherStationServer
{
    public class WeatherStationService : IWeatherStationService
    {
        static List<CurrentWeatherData> storedData = new List<CurrentWeatherData>
        {
            new CurrentWeatherData("Ruma", DateTime.Now.Date.AddHours(8), 15.4, 1012, 12.5, WindDirection.East, 0.0, 5.2, 85),
            new CurrentWeatherData("Ruma", DateTime.Now.Date.AddHours(18), 17.8, 1010, 8.3, WindDirection.SouthEast, 2.3, 4.1, 78),

            new CurrentWeatherData("Novi Sad", DateTime.Now.Date.AddDays(1).AddHours(9), 14.2, 1013, 10.7, WindDirection.NorthEast, 0.0, 6.1, 88),
            new CurrentWeatherData("Novi Sad", DateTime.Now.Date.AddDays(1).AddHours(20), 16.9, 1010, 7.1, WindDirection.West, 1.5, 3.3, 76),
            
            new CurrentWeatherData("Sremski Karlovci", DateTime.Now.Date.AddDays(2).AddHours(6), 13.3, 1015, 5.0, WindDirection.SouthWest, 0.0, 3.2, 90),
            new CurrentWeatherData("Sremski Karlovci", DateTime.Now.Date.AddDays(2).AddHours(17), 19.1, 1011, 10.3, WindDirection.North, 0.0, 7.6, 79),

            new CurrentWeatherData("Petrovaradin", DateTime.Now.Date.AddDays(3).AddHours(7), 16.5, 1014, 11.4, WindDirection.East, 0.1, 4.9, 81),
            new CurrentWeatherData("Petrovaradin", DateTime.Now.Date.AddDays(3).AddHours(19), 18.2, 1010, 9.3, WindDirection.South, 0.3, 5.4, 77),

            new CurrentWeatherData("Backa Topola", DateTime.Now.Date.AddDays(4).AddHours(8), 12.7, 1012, 7.6, WindDirection.NorthWest, 0.0, 2.1, 84),
            new CurrentWeatherData("Backa Topola", DateTime.Now.Date.AddDays(4).AddHours(17), 20.4, 1009, 6.8, WindDirection.West, 0.0, 8.4, 70)
        };

        public void Add(CurrentWeatherData data)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(data.Location.ToLower())
                                                                          && d.Timestamp.Date == data.Timestamp.Date   
                                                                          && d.Timestamp.Hour == data.Timestamp.Hour);
            if(existingData != null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA ALREADY EXISTS!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }

            storedData.Add(data);
            Console.WriteLine("1 NEW DATA ADDED: \n" + data);
        }

        public void Update(CurrentWeatherData data)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(data.Location.ToLower())
                                                                     && d.Timestamp.Date == data.Timestamp.Date 
                                                                     && d.Timestamp.Hour == data.Timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }

            existingData.Update(data);
            Console.WriteLine("1 DATA MODIFIED:\n " + data);
        }

        public CurrentWeatherData Get(DateTime timestamp, string location)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(location.ToLower())
                                                                  && d.Timestamp.Date == timestamp.Date 
                                                                  && d.Timestamp.Hour == timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }

            return existingData;
        }

        public double GetAverageHumidity(DateTime day, string location)
        {
            var filteredData = storedData.FindAll(d => d.Location.ToLower().Equals(location.ToLower()) 
                                                  && d.Timestamp.Date == day.Date);

            if (filteredData.Count == 0)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }

            return filteredData.Average(d => d.Humidity);

        }

        public double GetAverageTemperature(DateTime day, string location)
        {
            var filteredData = storedData.FindAll(d => d.Location.ToLower().Equals(location.ToLower()) 
                                                  && d.Timestamp.Date == day.Date);

            if (filteredData.Count == 0)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }

            return filteredData.Average(d => d.Temperature);
        }

        public int GetClearDaysNumber(DateTime month, string location)
        {
            var filteredData = storedData.FindAll(d => d.Location.ToLower().Equals(location.ToLower()) 
                                             && d.Timestamp.Year == month.Year 
                                             && d.Timestamp.Month == month.Month);
            if(filteredData.Count == 0)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }
            return filteredData.Where(d => d.Precipitation == 0).ToList().Count;
        }

        public List<int> GetExtremeUVIndexHours(DateTime day, string location)
        {
            var filteredData = storedData.FindAll(d => d.Location.ToLower().Equals(location.ToLower())
                                             && d.Timestamp.Date == day.Date);
            if (filteredData.Count == 0)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }

            double maxUVIndex = filteredData.Max(d => d.UVIndex);
            
            List<int> extremeHours = filteredData.Where(d => d.UVIndex == maxUVIndex).Select(d => d.Timestamp.Hour).Distinct().OrderBy(hour => hour).ToList();

            return extremeHours;
        }

        public double GetHumidity(DateTime timestamp, string location)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(location.ToLower())
                                                                  && d.Timestamp.Date == timestamp.Date
                                                                  && d.Timestamp.Hour == timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }
            return existingData.Humidity;
        }

        public List<CurrentWeatherData> GetInRange(DateTime from, DateTime to, string location)
        {
            var filteredData = storedData.FindAll(d => d.Location.ToLower() == location.ToLower() 
                                             && d.Timestamp >= from 
                                             && d.Timestamp <= to);
            if(filteredData.Count == 0)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }

            Console.WriteLine("FILTERED DATA IN RANGE: \n");
            foreach (CurrentWeatherData data in filteredData)
                Console.WriteLine(data);

            return filteredData;
        }

        public (double, double) GetMinMaxPrecitipation(DateTime day, string location)
        {
            var filteredData = storedData.FindAll(d => d.Location.ToLower() == location.ToLower()
                                                     && d.Timestamp.Date == day.Date);

            if (filteredData.Count == 0)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }

            double minPrecipitation = filteredData.Min(d => d.Precipitation);
            double maxPrecipitation = filteredData.Max(d => d.Precipitation);

            return (minPrecipitation, maxPrecipitation);
        }

        public (double, double) GetMinMaxTemperature(DateTime day, string location)
        {
            var filteredData = storedData.FindAll(d => d.Location.ToLower() == location.ToLower()
                                                     && d.Timestamp.Date == day.Date);

            if (filteredData.Count == 0)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }

            double minTemperature = filteredData.Min(d => d.Temperature);
            double maxTemperature = filteredData.Max(d => d.Temperature);

            return (minTemperature, maxTemperature);
        }

        public double GetPrecipitation(DateTime timestamp, string location)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(location.ToLower())
                                                                  && d.Timestamp.Date == timestamp.Date
                                                                  && d.Timestamp.Hour == timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }
            return existingData.Precipitation;
        }

        public double GetPressure(DateTime timestamp, string location)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(location.ToLower())
                                                                   && d.Timestamp.Date == timestamp.Date
                                                                   && d.Timestamp.Hour == timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }
            return existingData.Pressure;
        }

        public int GetRainyDaysNumber(DateTime month, string location)
        {
            var filteredData = storedData.FindAll(d => d.Location.ToLower().Equals(location.ToLower()) 
                                                  && d.Timestamp.Year == month.Year 
                                                  && d.Timestamp.Month == month.Month);
            if (filteredData.Count == 0)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);
            }
            return filteredData.Where(d => d.Precipitation > 0).ToList().Count;
        }

        public double GetTemperature(DateTime timestamp, string location)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(location.ToLower())
                                                                  && d.Timestamp.Date == timestamp.Date
                                                                  && d.Timestamp.Hour == timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }
            return existingData.Temperature;
        }

        public double GetUVIndex(DateTime timestamp, string location)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(location.ToLower())
                                                                  && d.Timestamp.Date == timestamp.Date
                                                                  && d.Timestamp.Hour == timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }
            return existingData.UVIndex;
        }

        public WindDirection GetWindDirection(DateTime timestamp, string location)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(location.ToLower())
                                                                  && d.Timestamp.Date == timestamp.Date
                                                                  && d.Timestamp.Hour == timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }
            return existingData.WindDirection;
        }

        public double GetWindSpeed(DateTime timestamp, string location)
        {
            CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(location.ToLower())
                                                                  && d.Timestamp.Date == timestamp.Date
                                                                  && d.Timestamp.Hour == timestamp.Hour);
            if (existingData == null)
            {
                WeatherDataServiceException ex = new WeatherDataServiceException()
                {
                    Reason = "DATA NOT FOUND!"
                };
                throw new FaultException<WeatherDataServiceException>(ex);

            }
            return existingData.WindSpeed;
        }

        public List<CurrentWeatherData> GetSince(DateTime replicationTime)
        {

            Console.WriteLine(DateTime.Now.ToString() + " -GetSince CALL.");
            List<CurrentWeatherData> filteredData = storedData.FindAll(d => d.LastModified > replicationTime);
          /*  foreach(CurrentWeatherData data in filteredData)
            {
                Console.WriteLine(data);
            }
          */
            return filteredData;
        }

        public void EnterData(List<CurrentWeatherData> newData)
        {
            Console.WriteLine("-EnterData INITIATED.");
            foreach(CurrentWeatherData data in newData)
            {
                CurrentWeatherData existingData = storedData.Find(d => d.Location.ToLower().Equals(data.Location.ToLower())
                                                                    && d.Timestamp.Date == data.Timestamp.Date);
                if(existingData != null)
                {
                    existingData.Update(data);
                }
                else
                {
                    storedData.Add(data);
                }

            }
        }
    }
}
