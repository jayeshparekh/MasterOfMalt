using System.Threading.Tasks;
using MasterOfMalt.Domain.Models;

namespace MasterOfMalt.Repository
{
    public interface IImagePropertiesProvider
    {
        Task<ImageDomainModel> GetImagePropertiesAsync(string file);
    }
}