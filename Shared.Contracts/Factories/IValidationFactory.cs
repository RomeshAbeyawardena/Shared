using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Shared.Contracts.Factories
{
    public interface IValidationFactory
    {
        IValidator<TModel> GetValidator<TModel>();
        ValidationResult Validate<TModel>(TModel model);
        Task<ValidationResult> ValidateAsync<TModel>(TModel model);
    }
}