using FluentValidation;
using System.Text.RegularExpressions;

using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Validations
{
    public class PoiValidator : AbstractValidator<PointOfInterest>
    {
        public PoiValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Trail name is required!")
                .MaximumLength(100).WithMessage("Trail name must not exceed 100 characters!");

            RuleFor(t => t.Mountain)
                .MaximumLength(100).WithMessage("Mountain name must not exceed 100 characters!");

            RuleFor(t => t.Latitude)
                .Must(l => Regex.IsMatch(l.ToString(), "^\\d+[.]\\d{5}$")).WithMessage("Latitude must be with format 12.34567!");

            RuleFor(t => t.Longitude)
                .Must(l => Regex.IsMatch(l.ToString(), "^\\d+[.]\\d{5}$")).WithMessage("Latitude must be with format 12.34567!");

            RuleFor(t => t.Description)
               .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters!");

            When(t => t is River, () =>
            {
                RuleFor(t => ((River)t).LengthKm)
                    .GreaterThan(0).WithMessage("Length must be greater than 0 kilometers!");
            });

            When(t => t is Lake, () =>
            {
                RuleFor(t => ((Lake)t).AreaKm2)
                    .GreaterThan(0).WithMessage("Area must be greater than 0 square kilometers!");
                RuleFor(t => ((Lake)t).DepthM)
                    .GreaterThan(0).WithMessage("Depth must be greater than 0 meters!");
            });

            When(t => t is Waterfall, () =>
            {
                RuleFor(t => ((Waterfall)t).HeightM)
                    .GreaterThan(0).WithMessage("Height must be greater than 0 meters!");
            });

            When(t => t is Peak, () =>
            {
                RuleFor(t => ((Peak)t).ElevationM)
                    .GreaterThan(0).WithMessage("Elevation must be greater than 0 meters!");
            });

            When(t => t is Monastery, () =>
            {
                RuleFor(t => ((Monastery)t).FoundedYear)
                    .GreaterThan(0).WithMessage("Founded year must be a positive number!");
            });

            When(t => t is Cave, () =>
            {
                RuleFor(t => ((Cave)t).LengthM)
                    .GreaterThan(0).WithMessage("Length must be greater than 0 meters!");
            });
        }
    }
}
