using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    [ServiceContract]
    public interface IWeatherStationService
    {
        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        void Add(CurrentWeatherData data);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        void Update(CurrentWeatherData data);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]

        CurrentWeatherData Get(DateTime timestamp, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]

        List<CurrentWeatherData> GetInRange(DateTime from, DateTime to, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        double GetTemperature(DateTime timestamp, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        double GetPressure(DateTime timestamp, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        double GetWindSpeed(DateTime timestamp, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        WindDirection GetWindDirection(DateTime timestamp, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        double GetPrecipitation(DateTime timestamp, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        double GetHumidity(DateTime timestamp, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        double GetUVIndex(DateTime timestamp, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        double GetAverageTemperature(DateTime day, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]    
        (double, double) GetMinMaxTemperature(DateTime day, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        int GetClearDaysNumber(DateTime month, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        int GetRainyDaysNumber(DateTime month, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        (double, double) GetMinMaxPrecitipation(DateTime day, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        double GetAverageHumidity(DateTime day, string location);


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        List<int> GetExtremeUVIndexHours(DateTime day, string location);    //u kojim sve satima je bilo extreme



    }
}
