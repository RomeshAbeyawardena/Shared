using Shared.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Shared.Library.Attributes;
using Shared.Contracts.Services;
using Shared.Domains.Enumerations;
using System.Threading.Tasks;

namespace Shared.Services.Providers
{
    public class DefaultDomainEncryptionProvider : IDomainEncryptionProvider
    {
        public async Task<TDest> Decrypt<TSource, TDest>(TSource value, SymmetricAlgorithmType symmetricAlgorithmType, byte[] initialVector)
        {
            var sourceProperties = value.GetType().GetProperties();
            var destProperties = typeof(TDest).GetProperties();
            var encryptionKeyProperties = GetPropertiesWithCustomAttribute<EncryptionKeyAttribute>(sourceProperties);
            var encryptionKeyProperty = encryptionKeyProperties.SingleOrDefault();

            var encryptedProperties = GetPropertiesWithCustomAttribute<EncryptableAttribute>(sourceProperties);

            var destination = Activator.CreateInstance<TDest>();
            var encryptionKey = encryptionKeyProperty.GetValue(value) as byte[]; 
            
            foreach(var property in encryptedProperties){
                var destinationProperty = GetProperty(destProperties, property.Name);
                var encryptedValue = property.GetValue(value) as IEnumerable<byte>;
                destinationProperty.SetValue(destination, await _encryptionService
                    .DecryptBytes(symmetricAlgorithmType, encryptedValue.ToArray(), encryptionKey, initialVector)
                    .ConfigureAwait(false));
            }

            return destination;
        }

        public async Task<TDest> Encrypt<TSource, TDest>(TSource value, SymmetricAlgorithmType symmetricAlgorithmType, byte[] key, byte[] salt, 
            byte[] initialVector, int iterations)
        {
            var sourceProperties = value.GetType().GetProperties();
            var destProperties = typeof(TDest).GetProperties();
            var encryptionKeyProperties = GetPropertiesWithCustomAttribute<EncryptionKeyAttribute>(destProperties);
            var encryptionKeyProperty = encryptionKeyProperties.SingleOrDefault();
            var destination = Activator.CreateInstance<TDest>();
            var generatedKey = _cryptographicProvider.GenerateKey(salt, key, iterations, 32);
            encryptionKeyProperty.SetValue(destination, generatedKey);

            foreach (var sourceProperty in sourceProperties)
            {
                var propertyValue = sourceProperty.GetValue(value);
                var destinationProperty = GetProperty(destProperties, sourceProperty.Name);
                if(sourceProperty.GetCustomAttribute<EncryptableAttribute>() != null && propertyValue != null)
                    destinationProperty?.SetValue(destination, await _encryptionService
                        .EncryptString(symmetricAlgorithmType, propertyValue.ToString(), generatedKey, initialVector)
                        .ConfigureAwait(false) );
                else
                    destinationProperty?.SetValue(destination, propertyValue);
            }

            return destination;
        }

        private PropertyInfo GetProperty(IEnumerable<PropertyInfo> properties, string propertyName)
        {
            return properties.SingleOrDefault(prop => prop.Name.Equals(propertyName, StringComparison.InvariantCulture));
        }

        private IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TCustomAttribute>(IEnumerable<PropertyInfo> properties)
            where TCustomAttribute : Attribute
        {
            return properties.Where(property => property.GetCustomAttribute<TCustomAttribute>() != null);
        }

        public DefaultDomainEncryptionProvider(ICryptographicProvider cryptographicProvider, IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _cryptographicProvider = cryptographicProvider;
        }

        private readonly IEncryptionService _encryptionService;
        private readonly ICryptographicProvider _cryptographicProvider;
    }
}
