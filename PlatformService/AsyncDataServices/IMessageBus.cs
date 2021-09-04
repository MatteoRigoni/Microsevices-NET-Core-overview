using platformservice.Dtos;

namespace platformservice.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublished);
    }
}