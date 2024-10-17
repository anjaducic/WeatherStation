using System.ServiceModel;
using SharedLibrary;


namespace DesktopClient
{
    public class ServiceManager
    {
        private ChannelFactory<IWeatherStationService> factorySource;
        private ChannelFactory<IWeatherStationService> factoryDestination;
        private ChannelFactory<IServerStateService> factoryPrimary;
        private ChannelFactory<IServerStateService> factorySecondary;

        private IWeatherStationService channelSource;
        private IWeatherStationService channelDestination;
        private IServerStateService channelPrimary;
        private IServerStateService channelSecondary;

        private EServerState primaryState = EServerState.Unknown;
        private EServerState secondaryState = EServerState.Unknown;

        public ServiceManager()
        {

        }

    }
}
