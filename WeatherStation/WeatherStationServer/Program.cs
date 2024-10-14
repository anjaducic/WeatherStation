using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WeatherStationServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceHost svc = new ServiceHost(typeof(WeatherStationService));
            svc.Open();

            Console.WriteLine("Press[Enter] to stop the server.");
            Console.ReadLine();
        }
    }
}
