using FluentValidation;

namespace Group4_GlassesShop.Models.Validation
{
    public class UpdateProfileValidation : AbstractValidator<Group4_GlassesShop.Models.UpdateProfile.UpdateProfileModel>
    {
        public UpdateProfileValidation()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Phone number must be in the correct format (10 digits)");

        }

    }
}
