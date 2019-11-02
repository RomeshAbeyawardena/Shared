using Microsoft.IO;
using Shared.Contracts;
using System.IO;

namespace Shared.Services
{
    public class MemoryStreamManager : IMemoryStreamManager
    {
        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;

        public MemoryStream GetStream()
        {
            return recyclableMemoryStreamManager.GetStream();
        }

        public MemoryStream GetStream(byte[] buffer)
        {
            var memoryStream = GetStream();
            memoryStream.Write(buffer);
            return memoryStream;
        }

        public MemoryStreamManager(RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            this.recyclableMemoryStreamManager = recyclableMemoryStreamManager;
        }
    }
}
