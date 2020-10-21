using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IActor 
    {
        Task InitializeAsync(string id);
        Task ShutdownAsync();
    }
}