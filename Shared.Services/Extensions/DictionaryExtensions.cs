using Shared.Contracts.Builders;
using System.Collections.Generic;
using System.Dynamic;

namespace Shared.Services.Extensions
{
    public static class DictionaryExtensions
    {
        public static object ToObject<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            dynamic expandoObject = dictionary as ExpandoObject;
            
            return expandoObject;
        }

        public static object ToObject<TKey, TValue>(this IDictionaryBuilder<TKey, TValue> dictionary)
        {
            dynamic expandoObject = dictionary as ExpandoObject;

            return expandoObject;
        }
    }
}
