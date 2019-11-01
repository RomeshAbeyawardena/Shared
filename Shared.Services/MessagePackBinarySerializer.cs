using System;
using System.IO;
using Shared.Contracts;
using Microsoft.IO;

namespace Shared.Services
{
    public class MessagePackBinarySerializer : IMessagePackBinarySerializer
    {
        public byte[] Serialize<T>(T value) where T : class
        {
            if(value == null)
                return Array.Empty<byte>();

            using (var memoryStream = recyclableMemoryStreamManager.GetStream())
            {

                MessagePack.MessagePackSerializer.Serialize(memoryStream, value);
                return memoryStream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] value) where T : class
        {
            if (value == null || value.Length == 0)
                return default;

            using (var memoryStream = recyclableMemoryStreamManager.GetStream()){
                memoryStream.Write(value);
                return MessagePack.MessagePackSerializer.Deserialize<T>(memoryStream);
            }
        }

        public MessagePackBinarySerializer(RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            this.recyclableMemoryStreamManager = recyclableMemoryStreamManager;
        }

        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
    }
}