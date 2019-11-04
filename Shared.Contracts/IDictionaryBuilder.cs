using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface IDictionaryBuilder<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IDictionaryBuilder<TKey, TValue> Add(KeyValuePair<TKey, TValue> keyValuePair);
        IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value);
        IDictionaryBuilder<TKey, TValue> Remove(TKey key);
    }
}
