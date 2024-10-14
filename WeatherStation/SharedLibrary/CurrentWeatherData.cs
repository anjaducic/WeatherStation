using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    [DataContract]
    public class CurrentWeatherData
    {
        [DataMember]
        public string Location { get => location; set => location = value; }
        [DataMember]
        public DateTime Timestamp { get  => timestamp; set => timestamp = value; }
        [DataMember]
        public double Temperature { get => temperature; set => temperature = value; }
        [DataMember]
        public double Pressure { get => pressure; set => pressure = value; }
        [DataMember]
        public double WindSpeed { get => windSpeed; set => windSpeed = value; }
        [DataMember]
        public WindDirection WindDirection { get => windDirection; set => windDirection = value; }
        [DataMember]
        public double Precipitation { get => precipitation; set => precipitation = value; }
        [DataMember]
        public double UVIndex { get => uvIndex; set => uvIndex = value; }
        [DataMember]
        public double Humidity { get => humidity; set => humidity = value; }


        private string location;
        private DateTime timestamp;
        private double temperature;
        private double pressure;
        private double windSpeed;
        private WindDirection windDirection;
        private double precipitation;
        private double uvIndex;
        private double humidity;
        private DateTime lastModified;

        public CurrentWeatherData(string location, DateTime timestamp, double temperature, double pressure, double windSpeed, WindDirection windDirection, double precipitation, double uvIndex, double humidity)
        {
            this.location = location;
            this.timestamp = timestamp;
            this.temperature = temperature;
            this.pressure = pressure;
            this.windSpeed = windSpeed;
            this.windDirection = windDirection;
            this.precipitation = precipitation;
            this.uvIndex = uvIndex;
            this.humidity = humidity;
            this.lastModified = DateTime.Now;

        }

        public void Update(CurrentWeatherData updated)
        {
            this.location = updated.Location;
            this.timestamp = updated.Timestamp;
            this.temperature = updated.Temperature;
            this.pressure = updated.Pressure;
            this.windSpeed = updated.WindSpeed;
            this.windDirection = updated.WindDirection;
            this.precipitation = updated.Precipitation;
            this.uvIndex = updated.UVIndex;
            this.humidity = updated.Humidity;
            this.lastModified = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Location: {this.location}\n" +
                   $"Timestamp: {this.timestamp:dd/MM/yyyy HH:mm:ss}\n" +
                   $"Temperature: {this.temperature}°C\n" +
                   $"Pressure: {this.pressure} hPa\n" +
                   $"Wind Speed: {this.windSpeed} km/h\n" +
                   $"Wind Direction: {this.windDirection}\n" +
                   $"Precipitation: {this.precipitation} mm\n" +
                   $"UV Index: {this.uvIndex}\n" +
                   $"Humidity: {this.humidity}%\n";
        }

    }

    [DataContract]
    public enum WindDirection
    {
        [EnumMemberAttribute]
        North = 0,
        [EnumMemberAttribute]
        NorthEast = 45,
        [EnumMemberAttribute]
        East = 90,
        [EnumMemberAttribute]
        SouthEast = 135,
        [EnumMemberAttribute]
        South = 180,
        [EnumMemberAttribute]
        SouthWest = 225,
        [EnumMemberAttribute]
        West = 270,
        [EnumMemberAttribute]
        NorthWest = 315     
    }

    

}
