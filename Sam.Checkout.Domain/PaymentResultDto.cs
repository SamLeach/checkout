namespace Sam.Checkout.Domain;

public class PaymentResultDto
{
    public required Guid Id { get; set; }

    public required bool Success { get; set; }
}
