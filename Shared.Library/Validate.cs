using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using Shared.Library.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Library
{
    public sealed class Validate<T> : IValidate<T>
    {
        public Validate(T model, IServiceProvider serviceProvider)
        {
            _model = model;
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly T _model;
        
        public IValidate<T> IsNotNull<TMember>(System.Func<T, TMember> getMember)
        {
            if(getMember == null)
                throw new ArgumentNullException(nameof(getMember));

            var member = getMember(_model);

            var memberName = nameof(member);
            if(member == null)
                throw new ValidateException(memberName, $"{memberName} must not be null");
            
            return this;
        }

        public IValidate<T> IsValid<TMember>(System.Func<T, TMember> getMember, TMember value, System.Func<TMember, TMember, bool> equalityComparer)
        {
            if(getMember == null)
                throw new ArgumentNullException(nameof(getMember));

            var member = getMember(_model);
            var memberName = nameof(member);

            if(!equalityComparer(member, value))
                throw new ValidateException(memberName, $"{memberName} invalid");
            return this;
        }

        public IValidate<T> IsInRange<TMember>(System.Func<T, TMember> getMember, TMember minimumValue, TMember maximumValue, System.Func<TMember, TMember, TMember, bool> isInRangeComparer)
        {
            if(getMember == null)
                throw new ArgumentNullException(nameof(getMember));

            var member = getMember(_model);
            var memberName = nameof(member);

            if(!isInRangeComparer(member, minimumValue, maximumValue))
                throw new ValidateException(memberName, $"{memberName} is not within the range of valid values");

            return this;
        }

        public IValidate<T> Regex(System.Func<T, string> getMember, string regexPattern)
        {
            if(getMember == null)
                throw new ArgumentNullException(nameof(getMember));

            var regex = new Regex(regexPattern);
            var member = getMember(_model);
            var memberName = nameof(member);
            
            if(!regex.IsMatch(member))
                throw new ValidateException(memberName, $"{memberName} does not match regex pattern");

            return this;
        }

        public IValidate<T> IsDuplicateEntry<TMember>(Func<T, TMember> getMember, Func<IServiceProvider, TMember, bool> checkDuplicateServiceComparer)
        {
            if(getMember == null)
                throw new ArgumentNullException(nameof(getMember));

            var member = getMember(_model);
            
            var memberName = nameof(member);

            if(checkDuplicateServiceComparer(_serviceProvider, member))
                throw new ValidateException(memberName, "Duplicate entries found.");

            return this;
        }

        public async Task<IValidate<T>> IsDuplicateEntryAsync<TMember>(Func<T, TMember> getMember, Func<IServiceProvider, TMember, Task<bool>> checkDuplicateServiceComparer)
        {
            if(getMember == null)
                throw new ArgumentNullException(nameof(getMember));

            if(checkDuplicateServiceComparer == null)
                throw new ArgumentNullException(nameof(checkDuplicateServiceComparer));

            var member = getMember(_model);
            
            var memberName = nameof(member);

            if(await checkDuplicateServiceComparer(_serviceProvider, member)
                .ConfigureAwait(false))
                throw new ValidateException(memberName, "Duplicate entries found.");

            return this;
        }

        public IValidate<T> IsDuplicateEntry(Func<IServiceProvider, T, bool> checkDuplicateServiceComparer)
        {
            if(checkDuplicateServiceComparer == null)
                throw new ArgumentNullException(nameof(checkDuplicateServiceComparer));

            if (checkDuplicateServiceComparer(_serviceProvider, _model))
                throw new ValidateException(nameof(_model), "Duplicate entries found.");

            return this;
        }

        public async Task<IValidate<T>> IsDuplicateEntryAsync(Func<IServiceProvider, T, Task<bool>> checkDuplicateServiceComparer)
        {
            if(checkDuplicateServiceComparer == null)
                throw new ArgumentNullException(nameof(checkDuplicateServiceComparer));

            if (await checkDuplicateServiceComparer(_serviceProvider, _model)
                .ConfigureAwait(false))
                throw new ValidateException(nameof(_model), "Duplicate entries found.");

            return this;
        }
    }
}