namespace Sam.Checkout.FakeAquiringBank;

public class PaymentRequest
{
    public Card Card { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }
}
