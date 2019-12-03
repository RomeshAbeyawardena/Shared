﻿using Shared.Library.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OptionalRequiredAttribute : ValidationAttribute
    {
        private readonly string[] _optionalMembers;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueType = validationContext.ObjectType;

            var nullMembersList = new List<string>();
            
            foreach (var optionalMember in _optionalMembers)
            {
                var valueProperty = valueType.GetProperty(optionalMember);

                if(valueProperty == null)
                    return new ValidationResult($"Unable to validate { optionalMember }");

                var valuePropertyValue = valueProperty
                    .GetValue(validationContext.ObjectInstance);
                
                if(valuePropertyValue.IsDefault())
                    nullMembersList.Add(optionalMember);
            }

            if(nullMembersList.Count == _optionalMembers.Length)
                return new ValidationResult("Parameter must not be null", nullMembersList.ToArray());

            return ValidationResult.Success;
        }

        public OptionalRequiredAttribute(params string[] optionalMembers)
        {
            _optionalMembers = optionalMembers;
        }
    }
}
