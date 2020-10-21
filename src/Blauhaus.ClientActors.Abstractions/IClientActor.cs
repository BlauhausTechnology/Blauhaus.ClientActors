using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IClientActor 
    {
        Task InitializeAsync(string id);
        Task ShutdownAsync();
    }
}