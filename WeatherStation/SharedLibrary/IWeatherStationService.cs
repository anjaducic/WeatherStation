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
        void Add(CurrentWeatherData data);
        [OperationContract]
        void Edit(CurrentWeatherData data);
        [OperationContract]
        CurrentWeatherData Get(DateTime timestamp, string location);
        [OperationContract]
        List<CurrentWeatherData> GetInRange(DateTime from, DateTime to, string location);
        [OperationContract]
        double GetTemperature(DateTime timestamp, string location);
        [OperationContract]
        double GetPressure(DateTime timestamp, string location);
        [OperationContract]
        double GetWindSpeed(DateTime timestamp, string location);
        [OperationContract]
        WindDirection GetWindDirection(DateTime timestamp, string location);
        [OperationContract]
        double GetPrecipitation(DateTime timestamp, string location);
        [OperationContract]
        double GetHumidity(DateTime timestamp, string location);
        [OperationContract]
        double GetUVIndex(DateTime timestamp, string location);
        [OperationContract]
        double GetAverageTemperature(DateTime day, string location);
        [OperationContract]
        (double, double) GetMinMaxTemperature(DateTime day, string location);
        [OperationContract]
        int GetClearDaysNumber(DateTime month, string location);
        [OperationContract]
        int GetRainyDaysNumber(DateTime month, string location);
        [OperationContract]
        (double, double) GetMinMaxPrecitipation(DateTime day, string location);
        [OperationContract]
        double GetAverageHumidity(DateTime day, string location);
        [OperationContract]
        List<int> GetExtremeUVIndexHours(DateTime day, string location);    //u kojim sve satima je bilo extreme



    }
}
