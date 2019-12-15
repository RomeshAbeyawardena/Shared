using DotNetInsights.Shared.Domains.Enumerations;
using System.Collections.Generic;

namespace DotNetInsights.Shared.Contracts
{
    public interface ICryptographicInfo
    {
        SymmetricAlgorithmType SymmetricAlgorithmType { get; }
        IEnumerable<byte> Key { get; }
        IEnumerable<byte> Salt { get; }
        IEnumerable<byte> InitialVector { get; }
        int Iterations { get; }
    }
}