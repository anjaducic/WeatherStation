using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedLibrary;

namespace Monitor
{
    public class Program
    {
        static ChannelFactory<IServerStateService> factoryPrimary;
        static ChannelFactory<IServerStateService> factorySecondary;
        static IServerStateService channelPrimary = null;
        static IServerStateService channelSecondary = null;
        static EServerState primaryState = EServerState.Unknown;
        static EServerState secondaryState = EServerState.Unknown;
        static bool primaryWorking = false;
        static bool secondaryWorking = false;


        static void Main(string[] args)
        {
            while (true)
            {
                ConnectToServices();
                CheckStates();
                SetServiceBToPrimary();
                ShowStates(); 
                Thread.Sleep(2000);
            }
        }

        private static void ConnectToServices()
        {
            try
            {
                factoryPrimary = new ChannelFactory<IServerStateService>("Primary");
                channelPrimary = factoryPrimary.CreateChannel();

                if ((secondaryState != EServerState.Primary && primaryWorking) || !secondaryWorking)
                {
                    channelPrimary.UpdateState(EServerState.Primary);
                }
                else if (primaryWorking && secondaryWorking)
                {
                    channelPrimary.UpdateState(EServerState.Secondary);    //if service A returns again
                    primaryState = EServerState.Secondary;
                }
                primaryWorking = true;
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("Primary service unavailable. Reason: " + cex.Message);
                primaryState = EServerState.Unknown;
                primaryWorking = false;
            }


            try
            {
                factorySecondary = new ChannelFactory<IServerStateService>("Secondary");
                channelSecondary = factorySecondary.CreateChannel();

                if (secondaryState == EServerState.Primary && secondaryWorking && primaryState != EServerState.Primary)
                {
                    channelSecondary.UpdateState(EServerState.Primary);
                }

                else if (primaryWorking && secondaryWorking)
                {
                    channelSecondary.UpdateState(EServerState.Secondary);
                    secondaryState = EServerState.Secondary;
                }
                secondaryWorking = true;

            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("Secondary service unavailable. Reason: " + cex.Message);
                secondaryState = EServerState.Unknown;
                secondaryWorking = false;
            }
        }

        private static void CheckStates()
        {
            try
            {
                primaryState = channelPrimary.GetState();
                primaryWorking = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SERVICE-A ERROR: " + ex.Message);
                primaryState = EServerState.Unknown;
                primaryWorking = false;
            }
            try
            {
                secondaryState = channelSecondary.GetState();
                secondaryWorking = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SERVICE-B ERROR: " + ex.Message);
                secondaryState = EServerState.Unknown;
                secondaryWorking = false;
            }
        }

        private static void SetServiceBToPrimary()
        {
            if (primaryState == EServerState.Unknown)
            {
                Thread.Sleep(500);

                if (secondaryState != EServerState.Primary)
                {
                    try
                    {
                        channelSecondary.UpdateState(EServerState.Primary);
                        secondaryState = EServerState.Primary;
                        Console.WriteLine("SERVICE-B STATUS BECAME: PRIMARY");
                        secondaryWorking = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SERVICE-B ERROR SWITCHING STATE: " + ex.Message);
                        secondaryState = EServerState.Unknown;
                        secondaryWorking = false;
                    }

                }
                else if (secondaryState == EServerState.Unknown)
                {
                    Console.WriteLine("BOTH SERVICES UNAVAILABLE. WAITING...");

                }
            }

            if (secondaryState == EServerState.Unknown)
            {
                Console.WriteLine("SERVICE-B ERROR.");
            }
        }

        private static void ShowStates()
        {
            Console.WriteLine("SERVICES STATES.\n");
            Console.WriteLine("-primary: " + primaryState.ToString());
            Console.WriteLine("-secondary: " + secondaryState.ToString());
        }

    }
}
