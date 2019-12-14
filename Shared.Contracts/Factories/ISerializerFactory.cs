using Shared.Domains.Enumerations;

namespace Shared.Contracts.Factories
{
    public interface ISerializerFactory
    {
        IBinarySerializer GetSerializer(SerializerType serializer);
    }
}
