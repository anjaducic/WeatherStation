using System;
using System.ServiceModel;
using System.Threading;
using SharedLibrary;

namespace Monitor
{
    public class Program
    {
        static IServerStateService primary = null;
        static IServerStateService secondary = null;

        static EServerState primaryState = EServerState.Unknown;
        static EServerState secondaryState = EServerState.Unknown;
        static bool primaryWorking = false;
        static bool secondaryWorking = false;

        static void Main(string[] args)
        {
            while (true)
            {
                MonitorPrimaryService();
                MonitorSecondaryService();
                UpdateServiceStates();
                HandleServiceStateSwitch();
                Thread.Sleep(2000); 
            }
        }

        static void MonitorPrimaryService()
        {
            try
            {
                ChannelFactory<IServerStateService> cfPrimary = new ChannelFactory<IServerStateService>("Primary");
                primary = cfPrimary.CreateChannel();

                if ((secondaryState != EServerState.Primary && primaryWorking) || !secondaryWorking)
                {
                    primary.UpdateState(EServerState.Primary);
                }
                else if (primaryWorking && secondaryWorking)
                {
                    primary.UpdateState(EServerState.Secondary); // Ako se vrati
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
        }

        static void MonitorSecondaryService()
        {
            try
            {
                ChannelFactory<IServerStateService> cfSecondary = new ChannelFactory<IServerStateService>("Secondary");
                secondary = cfSecondary.CreateChannel();

                if (secondaryState == EServerState.Primary && secondaryWorking && primaryState != EServerState.Primary)
                {
                    secondary.UpdateState(EServerState.Primary);
                }
                else if (primaryWorking && secondaryWorking)
                {
                    secondary.UpdateState(EServerState.Secondary);
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

        static void UpdateServiceStates()
        {
            try
            {
                primaryState = primary.GetState();
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
                secondaryState = secondary.GetState();
                secondaryWorking = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SERVICE-B ERROR: " + ex.Message);
                secondaryState = EServerState.Unknown;
                secondaryWorking = false;
            }
        }

        static void HandleServiceStateSwitch()
        {
            if (primaryState == EServerState.Unknown)
            {
                if (secondaryState == EServerState.Secondary)
                {
                    TrySwitchToPrimary();
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

            Console.WriteLine("SERVICES STATES.\n");
            Console.WriteLine("-primary: " + primaryState.ToString());
            Console.WriteLine("-secondary: " + secondaryState.ToString());
        }

        static void TrySwitchToPrimary()
        {
            try
            {
                secondary.UpdateState(EServerState.Primary);
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
    }
}
