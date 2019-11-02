using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface ISwitch<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        ISwitch<TKey, TValue> CaseWhen(TKey key, TValue value);
        TValue Case(TKey key);
    }
}
