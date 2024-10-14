using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace SharedLibrary
{
    [ServiceContract]
    public interface IServerStateService
    {
        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        EServerState GetState();


        [OperationContract]
        [FaultContract(typeof(WeatherDataServiceException))]
        void UpdateState(EServerState state);
    }

    [DataContract]
    public enum EServerState
    {
        [EnumMemberAttribute]
        Unknown,
        [EnumMemberAttribute]
        Primary,
        [EnumMemberAttribute]
        Secondary
    }
}
