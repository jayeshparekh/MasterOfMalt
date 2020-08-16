using MasterOfMalt.Domain.Exceptions;
using MasterOfMalt.Domain.Models;
using MasterOfMalt.Domain.Repository;
using System.Collections.Generic;

namespace MasterOfMalt.Repository
{
    public class GetImageQuery : IRepositoryQuery<IEnumerable<byte>>
    {
        private readonly string _imageRepositoryPath;
        private readonly IFileService _fileService;
        private readonly ImageDomainModel _imageInfo;

        public GetImageQuery(string imageRepositoryPath, IFileService fileService, ImageDomainModel imageInfo)
        {
            _imageRepositoryPath = imageRepositoryPath;
            _fileService = fileService;
            _imageInfo = imageInfo;
        }

        public IEnumerable<byte> Execute()
        {
            var file = $"{_imageRepositoryPath}\\{_imageInfo.Name}.{_imageInfo.Type}";

            if (!_fileService.DoesFileExist(file))
                throw new ImageNotFoundException(_imageInfo, $"Image {_imageInfo.Name} not found in repository!");

            var imageData = _fileService.ReadAllBytes(file);

            return imageData;
        }
    }
}
