using Shared.Contracts;
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

            throw new Va
            
            return this;
        }

        public IValidate<T> IsInRange<TMember>(System.Func<T, TMember> getMember, System.Func<TMember, TMember, bool> isInRangeComparer)
        {
            var member = getMember(_model);
            return this;
        }

        public IValidate<T> IsEqual<TMember>(System.Func<T, TMember> getMember, System.Func<TMember, TMember, bool> equalityComparer)
        {
            var member = getMember(_model);
            return this;
        }
    }
}