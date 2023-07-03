namespace Sam.Checkout.Domain;

public class PaymentEntity
{
    public Guid Id { get; set; }

    public Guid AquiringBankPaymentId { get; set; }

    public CardEntity Card { get; set; } = new CardEntity();

    public int Status { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }
}
