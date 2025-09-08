using FluentValidation;

using BulgarianMountainTrails.Core.DTOs;

namespace BulgarianMountainTrails.Core.Validations
{
    public class HutDtoValidator : AbstractValidator<HutDto>
    {
        public HutDtoValidator()
        {
        }
    }
}
