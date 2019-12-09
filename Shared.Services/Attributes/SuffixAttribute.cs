using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SuffixAttribute : Attribute
    {
        public SuffixAttribute(string suffix)
        {
            Suffix = suffix;
        }

        public string Suffix { get; }
    }
}
