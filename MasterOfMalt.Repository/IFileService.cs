using System.Collections.Generic;

namespace MasterOfMalt.Repository
{
    public interface IFileService
    {
        bool DoesFileExist(string file);
        
        IEnumerable<byte> ReadAllBytes(string file);

        IEnumerable<string> GetFiles(string path);

        bool DoesRepositoryDirectoryExist(string path);
    }
}