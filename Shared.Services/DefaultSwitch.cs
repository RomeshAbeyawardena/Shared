using Shared.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public static class DefaultSwitch
    {
        public static ISwitch<TKey, TValue> Create<TKey, TValue>()
        {
            return new Switch<TKey, TValue>();
        }
    }

    internal class Switch<TKey, TValue> : ISwitch<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _switchDictionary;

        public Switch()
        {
            _switchDictionary = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey key] => _switchDictionary[key];

        public IEnumerable<TKey> Keys => _switchDictionary.Keys;

        public IEnumerable<TValue> Values => _switchDictionary.Values;

        public int Count => _switchDictionary.Count;

        public TValue Case(TKey key)
        {
            if(_switchDictionary.TryGetValue(key, out var value))
                return value;

            throw new NullReferenceException($"Unable to find value for {key}");
        }

        public ISwitch<TKey, TValue> CaseWhen(TKey key, TValue value)
        {
            if(ContainsKey(key))
                throw new NullReferenceException("A value for {key} already exists");

            _switchDictionary.Add(key, value);
            return this;
        }

        public bool ContainsKey(TKey key)
        {
            return _switchDictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _switchDictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _switchDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _switchDictionary.GetEnumerator();
        }
    }
}
