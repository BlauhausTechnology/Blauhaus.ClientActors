using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{

    public interface IInitialize
    {
        Task InitializeAsync();
    }

    public interface IInitializeById 
    {
        Task InitializeAsync(string id);
    }
}