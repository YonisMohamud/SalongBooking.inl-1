using FluentValidation;
using SalongBooking.DTOs;

namespace SalongBooking.API.Validators;

public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingDtoValidator()
    {
        RuleFor(x => x.BookingDate)
            .NotEmpty().WithMessage("Bokningsdatum är obligatoriskt")
            .Must(BeInFuture).WithMessage("Bokningsdatum måste vara i framtiden");

        RuleFor(x => x.BookingTime)
            .NotEmpty().WithMessage("Bokningstid är obligatoriskt");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Kund-ID är obligatoriskt");

        RuleFor(x => x.HairdresserId)
            .GreaterThan(0).WithMessage("Frisör-ID är obligatoriskt");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0).WithMessage("Tjänst-ID är obligatoriskt");
    }

    private bool BeInFuture(DateTime date)
    {
        return date.Date >= DateTime.Today;
    }
}

