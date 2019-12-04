using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Library.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SkipFrom<T>(this IEnumerable<T> items, T item)
        {
            var itemArray = items.ToArray();
            var foundIndex  = Array.IndexOf(itemArray, item);

            if(foundIndex < 0)
                return items;

            return items.Skip(foundIndex + 1);
        }
    }
}
