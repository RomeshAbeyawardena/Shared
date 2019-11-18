using Microsoft.IO;
using Shared.Contracts;
using System.IO;

namespace Shared.Services
{
    public class MemoryStreamManager : IMemoryStreamManager
    {
        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;

        public MemoryStream GetStream(bool useRecyclableMemoryStreamManager = true, byte[] buffer = null)
        {
            MemoryStream ms = null;

            if (useRecyclableMemoryStreamManager)
                ms = recyclableMemoryStreamManager.GetStream();
            else
                ms = new MemoryStream();

            if(buffer != null && buffer.Length > 0)
                ms.Write(buffer);

            ms.Position = 0;

            return ms;
        }

        public MemoryStream GetStream(byte[] buffer, bool useRecyclableMemoryStreamManager = true)
        {
            var memoryStream = GetStream(useRecyclableMemoryStreamManager, buffer);
            return memoryStream;
        }

        public MemoryStreamManager(RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            this.recyclableMemoryStreamManager = recyclableMemoryStreamManager;
        }
    }
}
