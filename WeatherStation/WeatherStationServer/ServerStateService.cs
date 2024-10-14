using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace WeatherStationServer
{
    public class ServerStateService : IServerStateService
    {
        private static ServiceConfiguration configuration = new ServiceConfiguration(); 
        public EServerState GetState()
        {
            return ServerStateService.configuration.ServerState;
        }

        public void UpdateState(EServerState state)
        {
            ServerStateService.configuration.ServerState = state;
        }
    }
}
