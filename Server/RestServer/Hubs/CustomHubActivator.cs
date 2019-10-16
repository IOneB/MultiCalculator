using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;

namespace RestServer.Hubs
{
    /// <summary>
    /// Собственная реализация инстанцирования Хабов 
    /// </summary>
    public class CustomHubActivator : IHubActivator
    {
        private readonly Container _container;

        public CustomHubActivator(Container container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)_container.GetInstance(descriptor.HubType);
        }
    }
}
