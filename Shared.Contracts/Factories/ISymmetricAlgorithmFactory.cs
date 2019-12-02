using Shared.Domains.Enumerations;
using System.Security.Cryptography;

namespace Shared.Contracts.Factories
{
    public interface ISymmetricAlgorithmFactory
    {
        SymmetricAlgorithm GetSymmetricAlgorithm(SymmetricAlgorithmType symmetricAlgorithmType);
    }
}
