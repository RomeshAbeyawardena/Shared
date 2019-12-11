using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using Shared.Contracts.Factories;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Services.Factories
{
    public class DefaultValidationFactory : IValidationFactory
    {
        public IValidator<TModel> GetValidator<TModel>()
        {
            return _serviceProvider.GetRequiredService<IValidator<TModel>>();
        }

        public ValidationResult Validate<TModel>(TModel model)
        {
            var modelValidator = GetValidator<TModel>();
            return modelValidator.Validate(model);
        }

        public DefaultValidationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;
    }
}