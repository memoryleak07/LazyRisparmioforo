using FluentValidation;

namespace Risparmioforo.Services.ImportFileService;

public interface ICsvValidator : IValidator<ImportFileCommand> { }
public interface IImageValidator : IValidator<ImportFileCommand> { }

public class ImportFileValidators
{
    public class ImportFileCsvCommandValidator : AbstractValidator<ImportFileCommand>, ICsvValidator
    {
        public ImportFileCsvCommandValidator()
        {
            RuleFor(x => x.FileBytes)
                .NotNull().WithMessage("File stream cannot be null.");
        
            RuleFor(x => x.FileLength)
                .NotNull().WithMessage("File length cannot be null.")
                .LessThanOrEqualTo(10485760).WithMessage("File size must not exceed 10MB.");
        
            RuleFor(x => x.ContentType)
                .NotNull().WithMessage("Content type cannot be null.")
                .Must(x => x.Equals("text/csv")).WithMessage("Only csv file is supported.");
        }
    }

    public class ImportFileImageCommandValidator : AbstractValidator<ImportFileCommand>, IImageValidator
    {
        private readonly string[] _supportedExtensions = ["image/jpeg", "image/png"];
    
        public ImportFileImageCommandValidator()
        {
            RuleFor(x => x.FileBytes)
                .NotNull().WithMessage("File stream cannot be null.");
        
            RuleFor(x => x.FileLength)
                .NotNull().WithMessage("File length cannot be null.")
                .LessThanOrEqualTo(5 * 1024 * 1024).WithMessage("File size must not exceed 5MB.");
        
            RuleFor(x => x.ContentType)
                .Must(x => _supportedExtensions.Contains(x))
                .WithMessage("Only jpeg and png images are supported.");
        }
    }
}
