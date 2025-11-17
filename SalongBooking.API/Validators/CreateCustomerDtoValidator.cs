using FluentValidation;
using SalongBooking.DTOs;

namespace SalongBooking.API.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Namn är obligatoriskt")
            .MaximumLength(100).WithMessage("Namn får inte vara längre än 100 tecken");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-post är obligatoriskt")
            .EmailAddress().WithMessage("Ogiltig e-postadress")
            .MaximumLength(255).WithMessage("E-post får inte vara längre än 255 tecken");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Lösenord är obligatoriskt")
            .MinimumLength(6).WithMessage("Lösenord måste vara minst 6 tecken långt");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefonnummer är obligatoriskt")
            .MaximumLength(20).WithMessage("Telefonnummer får inte vara längre än 20 tecken");
    }
}

