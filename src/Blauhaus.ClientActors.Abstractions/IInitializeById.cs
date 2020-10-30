using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IInitializeById 
    {
        Task InitializeAsync(string id);
    }
}