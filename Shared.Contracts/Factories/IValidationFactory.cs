using System.ComponentModel.DataAnnotations;

namespace Shared.Contracts.Factories
{
    public interface IValidationFactory
    {
        IValidator<TModel> GetValidator<TModel>();
        ValidationResult Validate<TModel>(TModel model);
    }
}