using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using MasterOfMalt.Domain.Models;

namespace MasterOfMalt.Repository
{
    public class ImagePropertiesProvider : IImagePropertiesProvider
    {
        public Task<ImageDomainModel> GetImagePropertiesAsync(string file)
        {
            var fileInfo = new FileInfo(file);
            var image = Image.FromFile(file);

            var imageModel = new ImageDomainModel
            {
                Height = image.Height,
                Width = image.Width,
                Name = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf('.')),
                Type = fileInfo.Extension.Substring(1)
            };

            return Task.FromResult(imageModel);
        }
    }
}
