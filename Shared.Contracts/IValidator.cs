using System.ComponentModel.DataAnnotations;

namespace Shared.Contracts
{
    public interface IValidator<TModel> : IValidator
    {
        ValidationResult Validate(TModel model);
    }

    public interface IValidator
    {
        ValidationResult Validate(object model);
    }
}