using FluentValidation;

namespace Risparmioforo.Services.ImportFileService;

public class ImportFileCommandValidator : AbstractValidator<ImportFileCommand>
{
    public ImportFileCommandValidator()
    {
        RuleFor(x => x.FileStream)
            .NotNull().WithMessage("File stream cannot be null.");
        
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .MaximumLength(255).WithMessage("File name cannot exceed 255 characters.");
        
        RuleFor(x => x.FileLength)
            .NotNull().WithMessage("File length cannot be null.")
            .LessThanOrEqualTo(10485760).WithMessage("File size must not exceed 10MB.");
        
        RuleFor(x => x.ContentType)
            .NotNull().WithMessage("Content type cannot be null.")
            .Must(x => x.Equals("text/csv")).WithMessage("Only csv file is supported.");
    }
}