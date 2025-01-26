using FluentValidation;

namespace Resto_Backend.Model.Validation
{
    public class ItemValidator : AbstractValidator<ItemModel>
    {
        public ItemValidator()
        {
            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("Item Name is required");

            RuleFor(x => x.ItemDescription)
                .NotEmpty().WithMessage("Item Description is required");

            RuleFor(x => x.ItemPrice)
                .GreaterThan(0).WithMessage("Item Price must be greater than 0");

            RuleFor(x => x.CategoryID)
                .GreaterThan(0).WithMessage("Category ID is required and must be greater than 0");



            RuleFor(x => x.ItemImage)
                .NotEmpty().WithMessage("Item Image is required");

            RuleFor(x => x.ChefID)
                .GreaterThan(0).WithMessage("Chef ID is required and must be greater than 0");

   
        }
    }

}
