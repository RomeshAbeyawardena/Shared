using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Shared.Contracts;
using Shared.Domains;
using Shared.Library;
using Shared.Library.Extensions;

namespace Shared.Services
{
    public class DefaultCloner<T> : ICloner<T> where T : class
    {
        private readonly IBinarySerializer _binarySerializer;
        private readonly IMessagePackBinarySerializer _messagePackSerializer;
        private readonly DefaultCloneOptions _defaultCloneOptions;

        public DefaultCloner(IOptions<DefaultCloneOptions> cloneOptions, IBinarySerializer binarySerializer,
            IMessagePackBinarySerializer messagePackSerializer)
        {
            _binarySerializer = binarySerializer;
            _messagePackSerializer = messagePackSerializer;
            _defaultCloneOptions = cloneOptions.Setup();
        }

        public T Clone(T source, CloneType cloneType)
        {
            switch (cloneType)
            {
                case CloneType.Deep:
                    return DeepClone(source);
                case CloneType.Shallow:
                    return ShallowClone(source);
                default:
                    if(_defaultCloneOptions.DefaultCloneType == CloneType.Deep 
                       || _defaultCloneOptions.DefaultCloneType == CloneType.Shallow)
                        return Clone(source, _defaultCloneOptions.DefaultCloneType);
                    throw new NotSupportedException();
            }
        }

        public T ShallowClone(T source, IEnumerable<PropertyInfo> properties = null)
        {
            var sourceType = source.GetType();
            
            var newInstance = Activator.CreateInstance<T>();

            sourceType.ForProperties(source, newInstance, (propertyInfo, src, instance) =>
            {
                var value = propertyInfo.GetValue(src);

                if(value != null)
                    propertyInfo.SetValue(instance, value);
            });

            return newInstance;
        }

        public T DeepClone(T source)
        {
            return _defaultCloneOptions.UseMessagePack
                ? MessagePackDeepClone(source) 
                : BinaryDeepClone(source);
        }

        internal T BinaryDeepClone(T source)
        {
            if(typeof(T).GetCustomAttribute<SerializableAttribute>() == null)
                throw new NotSupportedException($"{nameof(T)} must have System.SerializableAttribute");

            var serialised = _binarySerializer.Serialize(source);
            return _binarySerializer.Deserialize<T>(serialised);
        }

        internal T MessagePackDeepClone(T source)
        {
            if (typeof(T).GetCustomAttribute<MessagePack.MessagePackObjectAttribute>() == null)
                throw new NotSupportedException($"{nameof(T)} must have System.MessagePackObjectAttribute");

            var serialised = _messagePackSerializer.Serialize(source);
            return _messagePackSerializer.Deserialize<T>(serialised);
        }
    }
}
