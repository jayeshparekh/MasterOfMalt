using System;

namespace MasterOfMalt.Domain.Exceptions
{
    public class ImageRepositoryNotFoundException : Exception
    {
        public ImageRepositoryNotFoundException(string message) : base(message){}
    }
}
