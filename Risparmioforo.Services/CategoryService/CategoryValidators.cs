using FluentValidation;
using Risparmioforo.Domain.Category;

namespace Risparmioforo.Services.CategoryService;

public interface ICategoryValidator : IValidator<Category> { }

public class CategoryValidator : AbstractValidator<Category>, ICategoryValidator
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Category name cannot be null.")
            .NotEmpty().WithMessage("Category name cannot be empty.")
            .MaximumLength(255).WithMessage("Category name maximum length is 255.");

        RuleFor(x => x.Flow)
            .NotNull().WithMessage("Flow cannot be null.")
            .IsInEnum().WithMessage("Flow is not valid.");
    }
}
