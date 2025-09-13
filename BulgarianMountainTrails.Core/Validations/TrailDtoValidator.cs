using FluentValidation;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data.Enums;

namespace BulgarianMountainTrails.Core.Validations
{
    public class TrailDtoValidator : AbstractValidator<TrailDto>
    {
        public TrailDtoValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Trail name is required!")
                .MaximumLength(100).WithMessage("Trail name must not exceed 100 characters!");

            RuleFor(t => t.Mountain)
                .NotEmpty().WithMessage("Mountain is required!")
                .MaximumLength(100).WithMessage("Mountain name must not exceed 100 characters!");

            RuleFor(t => t.Difficulty)
                .NotEmpty().WithMessage("Difficulty level is required!")
                .Must(d => Enum.TryParse<DifficultyEnum>(d, true, out _)).WithMessage("Invalid difficulty level! You can choose from: Unknown, Easy, Medium, Hard.");

            RuleFor(t => t.LengthKm)
                .GreaterThan(0).WithMessage("Length must be greater than 0 kilometers!");

            RuleFor(t => t.DurationHours)
               .GreaterThan(0).WithMessage("Duration must be greater than 0 hours!");

            RuleFor(t => t.StartPoint)
                .NotEmpty().WithMessage("Start Point is required!")
                .MaximumLength(200).WithMessage("Start point must not exceed 200 characters!");

            RuleFor(t => t.EndPoint)
                .NotEmpty().WithMessage("End Point is required!")
                .MaximumLength(200).WithMessage("End point must not exceed 200 characters!");

            RuleFor(t => t.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters!");
        }
    }
}
