using FluentValidation;
using LazyRisparmioforo.Domain.Commands;

namespace LazyRisparmioforo.Domain.Validators;

public class UploadFileViewModelValidator : AbstractValidator<UploadFileViewModel>
{
    public UploadFileViewModelValidator()
    {
        RuleFor(x => x.FormFile)
            .NotNull().WithMessage("File is required.");

        RuleFor(x => x.FormFile.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .MaximumLength(255).WithMessage("File name must be 255 characters or less.");

        RuleFor(x => x.FormFile.ContentType)
            .NotEmpty().WithMessage("Content type is required.")
            .Must(x => x.Contains("text/csv")).WithMessage("Invalid content type. Only CSV files are allowed.");

        RuleFor(x => x.FormFile.Length)
            .GreaterThan(0).WithMessage("File must not be empty.")
            .LessThanOrEqualTo(10 * 1024 * 1024).WithMessage("File size must be 10 MB or less.");
    }
}