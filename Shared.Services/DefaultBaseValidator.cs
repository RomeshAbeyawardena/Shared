using Shared.Contracts;
using Shared.Contracts.Factories;
using Shared.Library;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Shared.Services
{
    public abstract class DefaultBaseValidator<TModel> : IValidator<TModel>
    {
        public abstract ValidationResult Validate(TModel model);

        public virtual async Task<ValidationResult> ValidateAsync(TModel model)
        {
            await Task.CompletedTask.ConfigureAwait(false);
            return Validate(model);
        }

        ValidationResult IValidator.Validate(object model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            if(!(model is TModel tModel))
                throw new NotSupportedException();

            return Validate(tModel);
        }

        protected IValidate<TModel> ValidateModel(TModel model) => new Validate<TModel>(model, _serviceProvider);

        protected ValidationResult Success => ValidationResult.Success;

        protected ValidationResult Fail(string errorMessage, params string[] memberNames)
        {
            
            return new ValidationResult(errorMessage, memberNames);
        }

        protected async Task<ValidationResult> BaseValidateAsync(TModel model, ValidationResult validationResult)
        {
            if(validationResult == null)
                throw new ArgumentNullException(nameof(validationResult));

            var baseResult = await ValidateAsync(model).ConfigureAwait(false);
            if (baseResult != ValidationResult.Success)
                 return baseResult;

            return validationResult;
        }

        protected DefaultBaseValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;
    }
}