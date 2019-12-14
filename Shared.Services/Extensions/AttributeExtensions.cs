using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shared.Services.Extensions
{
    public static class AttributeExtensions
    {
        public static CustomAttributeData GetCustomArgumentData(this Type value, Type attributeType)
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value));

            return value.CustomAttributes.SingleOrDefault(customAttribute =>
                customAttribute.AttributeType == attributeType);
        }
    }
}
