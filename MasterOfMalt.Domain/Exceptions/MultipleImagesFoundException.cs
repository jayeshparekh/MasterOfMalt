using System;
using System.Collections.Generic;
using System.Text;
using MasterOfMalt.Domain.Models;

namespace MasterOfMalt.Domain.Exceptions
{
    public class MultipleImagesFoundException : Exception
    {
        public MultipleImagesFoundException(IEnumerable<ImageDomainModel> images, string message) : base(message)
        {
            Images = images;
        }

        public IEnumerable<ImageDomainModel> Images { get; }
    }
}
