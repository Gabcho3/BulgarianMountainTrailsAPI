using BulgarianMountainTrails.Core.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BulgarianMountainTrails.Core.Validations
{
    public class HutDtoValidator : AbstractValidator<HutDto>
    {
        public HutDtoValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Trail name is required!")
                .MaximumLength(100).WithMessage("Trail name must not exceed 100 characters!");

            RuleFor(t => t.Mountain)
                .NotEmpty().WithMessage("Mountain is required!")
                .MaximumLength(100).WithMessage("Mountain name must not exceed 100 characters!");

            RuleFor(t => t.Altitude)
                .GreaterThan(0).WithMessage("Altitude must be greater than 0 meters!");

            RuleFor(t => t.Capacity)
               .GreaterThan(0).WithMessage("Capacity must be more than 0 people!");

            RuleFor(t => t.PhoneNumber)
                .Must(p => Regex.IsMatch(p, "^\\s{0}|[+]359\\\\d{9}$")).WithMessage("PhoneNumber must be with format +359*** including 13 characters!");

            RuleFor(t => t.Latitude)
                .Must(l => Regex.IsMatch(l.ToString(), "^\\d+[.]\\d{5}$")).WithMessage("Latitude must be with format 12.34567!");

            RuleFor(t => t.Longitude)
                .Must(l => Regex.IsMatch(l.ToString(), "^\\d+[.]\\d{5}$")).WithMessage("Latitude must be with format 12.34567!");
        }
    }
}
