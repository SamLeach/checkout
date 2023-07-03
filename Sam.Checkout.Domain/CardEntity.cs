namespace Sam.Checkout.Domain;
public class CardEntity
{
    public Guid Id { get; set; }

    public string MaskedNumber { get; set; }

    public int ExpiryMonth { get; set; }

    public int ExpiryYear { get; set; }

    public string Cvv { get; set; }
}
