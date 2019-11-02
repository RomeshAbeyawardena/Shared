using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Library.Extensions
{
    public static class ObjectCollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> iterator)
        {
            foreach (var item in collection)
            {
                iterator(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> iterator)
        {
            var index = 0;
            foreach (var item in collection)
            {
                iterator(item, index++);
            }
        }

        public static async Task ForEach<T>(this IEnumerable<T> collection, Func<T, Task> asyncIterator)
        {
            foreach (var item in collection)
            {
                await asyncIterator(item);
            }
        }

        public static async Task ForEach<T>(this IEnumerable<T> collection, Func<T, int, Task> asyncIterator)
        {
            var index = 0;
            foreach (var item in collection)
            {
                await asyncIterator(item, index++);
            }
        }
    }
}
