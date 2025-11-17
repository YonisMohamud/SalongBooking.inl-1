using FluentValidation;
using SalongBooking.DTOs;

namespace SalongBooking.API.Validators;

public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Namn är obligatoriskt")
            .MaximumLength(100).WithMessage("Namn får inte vara längre än 100 tecken");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefonnummer är obligatoriskt")
            .MaximumLength(20).WithMessage("Telefonnummer får inte vara längre än 20 tecken");
    }
}

