using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MasterOfMalt.Api.Services;
using MasterOfMalt.Domain.Exceptions;
using MasterOfMalt.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MasterOfMalt.Api.Controllers
{
    [Route("api/mom/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageDirectoryService _imageDirectoryService;
        private readonly IFileService _fileService;
        private readonly string _imageRepositoryPath;

        public ImageController(IImageDirectoryService imageDirectoryService, IConfiguration configuration, IFileService fileService)
        {
            _imageRepositoryPath = configuration.GetValue<string>("ImageRepositoryPath");
            _imageDirectoryService = imageDirectoryService;
            _fileService = fileService;
        }

        [Route("{name}/height/{height:int}/width/{width:int}/type/{type}")]
        [Route("{name}/height/{height:int}/width/{width:int}/type/{type}/background/{background}")]
        [Route("{name}/height/{height:int}/width/{width:int}/type/{type}/watermark/{watermark}")]
        [Route("{name}/height/{height:int}/width/{width:int}/type/{type}/background/{background}/watermark/{watermark}")]
        [HttpGet]
        public IActionResult GetImage(
            string name,
            int height,
            int width,
            string type,
            string background = null,
            string watermark = null)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest($"Parameter {nameof(name)} is invalid!");
            if (string.IsNullOrEmpty(type))
                return BadRequest($"Parameter {nameof(type)} is invalid!");
            if (height <= 0)
                return BadRequest($"Parameter {nameof(height)} is invalid!");
            if (width <= 0)
                return BadRequest($"Parameter {nameof(width)} is invalid!");

            try
            {
                var imageInfo = _imageDirectoryService.GetImageInfo(name, height, width, type, background, watermark);

                if (imageInfo == null)
                    return NotFound();

                var getImageQuery = new GetImageQuery(_imageRepositoryPath, _fileService, imageInfo);
                var image = getImageQuery.Execute();

                return File(image.ToArray(), $"image/{imageInfo.Type}");

            }
            catch (ImageRepositoryNotFoundException exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
            catch (MultipleImagesFoundException exception)
            {
                return Conflict(exception.Message);
            }
            catch (ImageNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
