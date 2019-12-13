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

        protected DefaultBaseValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;
    }
}