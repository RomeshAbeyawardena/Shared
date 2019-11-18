using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts.Providers
{
    public interface IListBuilder<T> : IReadOnlyList<T>
    {
        IListBuilder<T> Add(T value);
        IListBuilder<T> AddRange(IEnumerable<T> value);
        IListBuilder<T> Insert(T value, int index);
    }
}
