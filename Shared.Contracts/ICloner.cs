using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Shared.Domains;

namespace Shared.Contracts
{
    public interface ICloner<T> where T: class
    {
        T Clone(T source, CloneType cloneType);
        T ShallowClone(T source, IEnumerable<PropertyInfo> properties = null);
        T DeepClone(T source);
    }
}
