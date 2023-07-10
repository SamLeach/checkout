using FluentValidation;

namespace Sam.Checkout.Domain;

public class CardDto
{
    public required string Number { get; set; }

    public required int ExpiryMonth { get; set; }

    public required int ExpiryYear { get; set; }

    public required string Cvv { get; set; } // TODO: Should probaby be an int or short int
}

public class CardDtoValidator : AbstractValidator<CardDto>
{
    // There are lots more validation to be done. Don't have time, we can chat about it.
    public CardDtoValidator()
    {
        RuleFor(x => x.Number).Length(16, 16); // I know not all cards are 16 digits but keeping it simple
        RuleFor(x => x.ExpiryMonth).InclusiveBetween(1, 12);
        RuleFor(x => x.ExpiryYear).LessThanOrEqualTo(99);
        RuleFor(x => x.Cvv).Length(3, 3);
    }
}
