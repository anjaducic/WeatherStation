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
        static IServerStateService primary = null;
        static IServerStateService secondary = null;

        static void Main(string[] args)
        {
            EServerState primaryState = EServerState.Unknown;
            EServerState secondaryState = EServerState.Unknown;

            try
            {
                ChannelFactory<IServerStateService> cfPrimary = new ChannelFactory<IServerStateService>("Primary");
                primary = cfPrimary.CreateChannel();
                primary.UpdateState(EServerState.Primary);
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("Primary service unavailable. Reason:" + cex.Message);
            }


            try
            {
                ChannelFactory<IServerStateService> cfSecondary = new ChannelFactory<IServerStateService>("Secondary");
                secondary = cfSecondary.CreateChannel();
                secondary.UpdateState(EServerState.Secondary);
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("Secondary service unavailable. Reason:" + cex.Message);

            }

            while (true)
            {
                try
                {
                    ChannelFactory<IServerStateService> cfPrimary = new ChannelFactory<IServerStateService>("Primary");
                    primary = cfPrimary.CreateChannel();

                    if (secondaryState != EServerState.Primary)
                    {
                        primary.UpdateState(EServerState.Primary);

                    }
                    else
                    {
                        primary.UpdateState(EServerState.Secondary);    //ako se vrati
                        primaryState = EServerState.Secondary;
                    }
                }
                catch (CommunicationException cex)
                {
                    Console.WriteLine("Primary service unavailable. Reason:" + cex.Message);
                }


                try
                {
                    ChannelFactory<IServerStateService> cfSecondary = new ChannelFactory<IServerStateService>("Secondary");
                    secondary = cfSecondary.CreateChannel();

                    if (secondaryState == EServerState.Primary)
                    {
                        secondary.UpdateState(EServerState.Primary);
                    }
                        
                    else
                    {
                        secondary.UpdateState(EServerState.Secondary);
                        secondaryState = EServerState.Secondary;
                    }
                        
                }
                catch (CommunicationException cex)
                {
                    Console.WriteLine("Secondary service unavailable. Reason:" + cex.Message);

                }

                

                try
                {
                    primaryState = primary.GetState();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SERVICE-A ERROR: " + ex.Message);
                    primaryState = EServerState.Unknown;
                }
                try
                {
                    secondaryState = secondary.GetState();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SERVICE-B ERROR: " + ex.Message);
                    secondaryState = EServerState.Unknown;
                }




                if (primaryState == EServerState.Unknown)
                {
                    if (secondaryState == EServerState.Secondary)
                    {
                        secondary.UpdateState(EServerState.Primary);
                        secondaryState = EServerState.Primary;
                        Console.WriteLine("SERVICE-B STATUS BECAME: PRIMARY");
                    }
                    else if (secondaryState == EServerState.Unknown)
                    {
                        Console.WriteLine("BOTH SERVICES UNAVAILABLE. WAITING...");
                        continue;
                    }
                }

                if (secondaryState == EServerState.Unknown)
                {
                    Console.WriteLine("SERVICE-B ERROR.");
                }

                Console.WriteLine("SERVICES STATES.\n");
                Console.WriteLine("-primary: " + primaryState.ToString());
                Console.WriteLine("-secondary: " + secondaryState.ToString());


                Thread.Sleep(3000);
            }

        }

    }
}
