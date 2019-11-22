using System.Collections.Generic;

namespace Shared.Contracts
{
    public interface ISwitch<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        ISwitch<TKey, TValue> CaseWhen(TKey key, TValue value, params TKey[] aliases);
        TValue Case(TKey key);
    }
}
