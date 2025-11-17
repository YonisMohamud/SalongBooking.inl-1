using FluentValidation;
using SalongBooking.DTOs;

namespace SalongBooking.API.Validators;

public class CreateHairdresserDtoValidator : AbstractValidator<CreateHairdresserDto>
{
    public CreateHairdresserDtoValidator()
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

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialisering är obligatoriskt")
            .MaximumLength(100).WithMessage("Specialisering får inte vara längre än 100 tecken");
    }
}

