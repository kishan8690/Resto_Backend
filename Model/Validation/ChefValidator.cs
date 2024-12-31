using FluentValidation;

namespace Resto_Backend.Model.Validation
{
    public class ChefValidator:AbstractValidator<ChefModel>
    {
        public ChefValidator() 
        {
            RuleFor(x => x.ChefName).NotEmpty().WithMessage("Chef Name is required");
            RuleFor(x => x.ChefSpeciality).NotEmpty().WithMessage("Chef Speciality is required");
            RuleFor(x => x.ChefImage).NotEmpty().WithMessage("Chef Image is required");
            RuleFor(x => x.Experience).NotEmpty().WithMessage("Experience is required");
            RuleFor(x => x.MobileNumber).NotEmpty().MinimumLength(10).MaximumLength(10).WithMessage("Mobile Number is required");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(x => x.Designation).NotEmpty().WithMessage("Designation is required");
            RuleFor(x => x.Salary).NotEmpty().WithMessage("Salary is required");
        }
    }
}
