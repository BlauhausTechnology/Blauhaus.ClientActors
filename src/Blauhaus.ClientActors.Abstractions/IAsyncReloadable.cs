using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IAsyncReloadable
    {
        Task ReloadAsync();
    }
}