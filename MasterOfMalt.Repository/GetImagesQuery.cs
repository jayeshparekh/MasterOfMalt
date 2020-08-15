using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterOfMalt.Domain.Models;
using MasterOfMalt.Domain.Repository;

namespace MasterOfMalt.Repository
{
    public class GetImagesQuery : IRepositoryQuery<IEnumerable<ImageDomainModel>>
    {
        private readonly string _imageRepositoryPath;
        private readonly IFileService _fileService;
        private readonly IImagePropertiesProvider _imageDataProvider;

        public GetImagesQuery(string imageRepositoryPath, IFileService fileService, IImagePropertiesProvider imageDataProvider)
        {
            _imageRepositoryPath = imageRepositoryPath;
            _fileService = fileService;
            _imageDataProvider = imageDataProvider;
        }

        public IEnumerable<ImageDomainModel> Execute()
        {
            var imageFiles = _fileService.GetFiles(_imageRepositoryPath);

            var tasks = imageFiles
                .Select(f => _imageDataProvider.GetImagePropertiesAsync(f))
                .ToArray();

            Task.WaitAll(tasks);

            var images = tasks.Select(t => t.Result);

            return images;
        }
    }
}