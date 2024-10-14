using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    [DataContract]
    public class WeatherDataServiceException
    {
        [DataMember]
        public string Reason { get => reason; set => reason = value; }

        private string reason;
    }
}
