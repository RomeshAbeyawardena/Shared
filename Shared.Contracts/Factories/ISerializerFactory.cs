using Shared.Domains;

namespace Shared.Contracts.Factories
{
    public interface ISerializerFactory
    {
        IBinarySerializer GetSerializer(SerializerType serializer);
    }
}
