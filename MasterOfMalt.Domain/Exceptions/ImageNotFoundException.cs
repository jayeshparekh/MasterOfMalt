using MasterOfMalt.Domain.Models;
using System;

namespace MasterOfMalt.Domain.Exceptions
{
    public class ImageNotFoundException : Exception
    {
        public ImageNotFoundException(ImageDomainModel image, string message) : base(message)
        {
            Image = image;
        }
        
        public ImageDomainModel Image { get; }
    }
}
