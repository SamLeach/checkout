using FluentValidation;

namespace Sam.Checkout.Domain;

public class PaymentDto
{
    public required CardDto Card { get; set; }

    public required decimal Amount { get; set; }

    // I could create a custom validator or Enum, etc. But using a string due to time constrainsts
    public required string Currency { get; set; }
}

public class PaymentDtoValidator : AbstractValidator<PaymentDto>
{
    // There are lots more validation to be done. Don't have time, we can chat about it.
    public PaymentDtoValidator()
    {
        RuleFor(x => x.Card).NotNull();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).Length(3);
    }
}