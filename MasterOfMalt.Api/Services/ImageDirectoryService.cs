using MasterOfMalt.Domain.Exceptions;
using MasterOfMalt.Domain.Models;
using MasterOfMalt.Repository;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace MasterOfMalt.Api.Services
{
    public class ImageDirectoryService : IImageDirectoryService
    {
        private readonly IFileService _fileService;
        private readonly IImagePropertiesProvider _imagePropertiesProvider;
        private readonly string _imageRepositoryPath;
        
        private IEnumerable<ImageDomainModel> _images;

        public ImageDirectoryService(
            IFileService fileService, 
            IImagePropertiesProvider imagePropertiesProvider,
            IConfiguration configuration)
        {
            _imageRepositoryPath = configuration.GetValue<string>("ImageRepositoryPath");
            _fileService = fileService;
            _imagePropertiesProvider = imagePropertiesProvider;

            Init();
        }

        public ImageDomainModel GetImageInfo(string name, int height, int width, string type, string backgroundColor, string watermark)
        {
            var filteredImages = _images
                .Where(i => i.Name == name && i.Height == height && i.Width == width && i.Type == type);

            if (!string.IsNullOrEmpty(backgroundColor))
            {
                filteredImages = filteredImages.Where(i => i.Background == backgroundColor);
            }
            if (!string.IsNullOrEmpty(watermark))
            {
                filteredImages = filteredImages.Where(i => i.WaterMark == watermark);
            }

            if (!filteredImages.Any())
                return null;

            if (filteredImages.Count() > 1)
                throw new MultipleImagesFoundException(filteredImages, "Multiple images discovered using search parameters!");

            return filteredImages.First();
        }

        private void Init()
        {
            if (!_fileService.DoesRepositoryDirectoryExist(_imageRepositoryPath))
                throw new ImageRepositoryNotFoundException($"Image repository Not found!");

            var getImagesQuery = new GetImagesQuery(_imageRepositoryPath, _fileService, _imagePropertiesProvider);
            _images = getImagesQuery.Execute();
        }
    }
}
