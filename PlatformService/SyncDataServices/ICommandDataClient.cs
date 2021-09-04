using System.Threading.Tasks;
using PlatformService.Dtos;

namespace platformservice.SyncDataServices
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto platformDto);
    }
}