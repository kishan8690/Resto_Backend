using FluentValidation;

namespace Resto_Backend.Model.Validation
{
    public class UserValidator:AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Mimimum Length is 6").WithMessage("Password is required");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
            RuleFor(x => x.MobileNumber).NotEmpty().MinimumLength(10).MaximumLength(10).WithMessage("Mobile Number is required");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(x => x.ProfileImage).NotEmpty().WithMessage("Profile Image is required");
        }
    }
}
