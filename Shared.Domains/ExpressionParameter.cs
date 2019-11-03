using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domains
{
    public class ExpressionParameter
    {
        public bool? IsNull {get;set;}
        public string Name {get;set;}
        public object Value { get; set; }
        public ExpressionCondition Condition {get;set;}
    }
}
