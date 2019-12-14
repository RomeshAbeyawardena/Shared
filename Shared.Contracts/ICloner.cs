using System.Collections.Generic;
using System.Reflection;
using Shared.Domains.Enumerations;

namespace Shared.Contracts
{
    public interface ICloner<T> where T: class
    {
        T Clone(T source, CloneType cloneType);
        T ShallowClone(T source, IEnumerable<PropertyInfo> properties = null);
        T DeepClone(T source);
    }
}
