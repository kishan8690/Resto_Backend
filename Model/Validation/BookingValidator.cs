using FluentValidation;
using Resto_Backend.Models;

namespace Resto_Backend.Model.Validation
{
    public class BookingValidator : AbstractValidator<BookingModel>
    {
        public BookingValidator()
        {
           

            RuleFor(x => x.BookingDate)
                .NotEmpty().WithMessage("Booking Date is required")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Booking Date cannot be in the past");

            RuleFor(x => x.UserID)
                .GreaterThan(0).WithMessage("User ID is required and must be greater than 0");

           

            RuleFor(x => x.NumberOfPerson)
                .GreaterThan(0).WithMessage("Number of Persons must be greater than 0");

        }
    }

}
