using System.IO;

namespace Shared.Contracts
{
    public interface IMemoryStreamManager
    {
        MemoryStream GetStream();
        MemoryStream GetStream(byte[] buffer);
    }
}
