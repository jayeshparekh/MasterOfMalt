using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace MasterOfMalt.Repository
{
    public class FileService : IFileService
    {
        private static readonly ReaderWriterLockSlim ReaderWriterLockSlim = new ReaderWriterLockSlim();

        public bool DoesFileExist(string file)
        {
            return File.Exists(file);
        }

        public IEnumerable<byte> ReadAllBytes(string file)
        {
            ReaderWriterLockSlim.EnterReadLock();

            try
            {
                var data = File.ReadAllBytes(file);
                return data;
            }
            finally
            {
                ReaderWriterLockSlim.ExitReadLock();
            }
        }

        public IEnumerable<string> GetFiles(string path)
        {
            var files = Directory.GetFiles(path);
            return files;
        }

        public bool DoesRepositoryDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }
    }
}
