using Shared.Domains;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Contracts.Factories
{
    public interface ISymmetricAlgorithmFactory
    {
        SymmetricAlgorithm GetSymmetricAlgorithm(SymmetricAlgorithmType symmetricAlgorithmType);
    }
}
