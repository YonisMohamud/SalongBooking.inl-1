using FluentValidation;
using SalongBooking.DTOs;

namespace SalongBooking.API.Validators;

public class CreateServiceDtoValidator : AbstractValidator<CreateServiceDto>
{
    public CreateServiceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Namn är obligatoriskt")
            .MaximumLength(100).WithMessage("Namn får inte vara längre än 100 tecken");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Pris måste vara större än 0");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Varaktighet måste vara större än 0");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Beskrivning får inte vara längre än 500 tecken");
    }
}

