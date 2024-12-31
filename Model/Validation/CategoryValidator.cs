using FluentValidation;

namespace Resto_Backend.Model.Validation
{
    public class CategoryValidator:AbstractValidator<CategoryModel>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category Name is required");
        }
    }
}
