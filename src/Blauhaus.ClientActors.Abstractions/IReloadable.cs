using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IReloadable
    {
        Task ReloadAsync();
    }
}