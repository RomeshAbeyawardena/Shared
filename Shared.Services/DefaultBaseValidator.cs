using Shared.Contracts;
using Shared.Contracts.Factories;
using Shared.Library;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Services
{
    public abstract class DefaultBaseValidator<TModel> : IValidator<TModel>
    {
        public abstract ValidationResult Validate(TModel model);

        ValidationResult IValidator.Validate(object model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            if(!(model is TModel tModel))
                throw new NotSupportedException();

            return Validate(tModel);
        }

        protected IValidate<TModel> ValidateModel(TModel model) => new Validate<TModel>(model);

        protected ValidationResult Fail(string errorMessage, params string[] memberNames)
        {
            
            return new ValidationResult(string.Empty, memberNames);
        }
    }
}