using MasterOfMalt.Domain.Models;

namespace MasterOfMalt.Api.Services
{
    public interface IImageInfoCache
    {
        ImageDomainModel GetImageInfo(string name, int height, int width, string type, string backgroundColor, string watermark);
    }
}