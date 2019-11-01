using System;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.IO;
using Shared.Contracts;

namespace Shared.Services
{
    public class BinarySerializer : IBinarySerializer
    {
        private readonly BinaryFormatter binaryFormatter;

        public virtual byte[] Serialize<T>(T value) where T : class
        {
            if(value == null)
                return Array.Empty<byte>();

            using (var memoryStream = recyclableMemoryStreamManager.GetStream()){
                binaryFormatter.Serialize(memoryStream, value);
                return memoryStream.ToArray();
            }
        }

        public virtual T Deserialize<T>(byte[] value) where T : class
        {
            if (value == null || value.Length == 0)
                return default;

            using (var memoryStream = recyclableMemoryStreamManager.GetStream()){
                memoryStream.Write(value);
                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }

        public BinarySerializer(RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            this.recyclableMemoryStreamManager = recyclableMemoryStreamManager;
            binaryFormatter = new BinaryFormatter();
        }

        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
    }
}
