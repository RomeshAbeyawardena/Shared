using Shared.Contracts;
using Shared.Library.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Shared.Library
{
    public sealed class Validate<T> : IValidate<T>
    {
        public Validate(T model)
        {
            _model = model;
        }

        private readonly T _model;
        
        public IValidate<T> IsNotNull<TMember>(System.Func<T, TMember> getMember)
        {
            var member = getMember(_model);

            var memberName = nameof(member);
            if(member == null)
                throw new ValidateException(memberName, $"{memberName} must not be null");
            
            return this;
        }

        public IValidate<T> IsValid<TMember>(System.Func<T, TMember> getMember, TMember value, System.Func<TMember, TMember, bool> equalityComparer)
        {
            var member = getMember(_model);
            var memberName = nameof(member);

            if(!equalityComparer(member, value))
                throw new ValidateException(memberName, $"{memberName} invalid");
            return this;
        }

        public IValidate<T> IsInRange<TMember>(System.Func<T, TMember> getMember, TMember minimumValue, TMember maximumValue, System.Func<TMember, TMember, TMember, bool> isInRangeComparer)
        {
            var member = getMember(_model);
            var memberName = nameof(member);

            if(!isInRangeComparer(member, minimumValue, maximumValue))
                throw new ValidateException(memberName, $"{memberName} is not within the range of valid values");

            return this;
        }
    }
}