using FluentValidation;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Queries;

namespace LazyRisparmioforo.Domain.Validators;

public class StatisticValidators
{
    public class StatisticCommandValidator : AbstractValidator<DateRangeQuery>
    {
        public StatisticCommandValidator()
        {
            RuleFor(x => x.FromDate)
                .NotNull().WithMessage("From date cannot be null.")
                .Must(BeValidDate).WithMessage("From date must be a valid date.");
            
            RuleFor(x => x.ToDate)
                .NotNull().WithMessage("End date cannot be null.")
                .Must(BeValidDate).WithMessage("To date must be a valid date.");
           
            RuleFor(x => x)
                .Must(x => x.FromDate <= x.ToDate)
                .WithMessage("From date must be less than or equal to To date.");
        }

        private bool BeValidDate(DateOnly date) => 
            date != DateOnly.MinValue && date != DateOnly.MaxValue && date != default;
    }
}