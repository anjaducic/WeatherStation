using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace WeatherStationServer
{
    public class ServiceConfiguration
    {
        public EServerState ServerState { get => serverState; set => serverState = value; }

        private EServerState serverState;


        public ServiceConfiguration()
        {
            this.serverState = (EServerState)Enum.Parse(typeof(EServerState), Properties.Settings.Default.ServerState);

            Console.WriteLine("Server: " + this.serverState.ToString());
        }
    }
}
