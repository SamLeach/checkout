namespace Sam.Checkout.Domain;
public class PaymentQuery : IPaymentQuery
{
    private readonly IPaymentRepository paymentRepository;

    public PaymentQuery(IPaymentRepository paymentRepository)
    {
        this.paymentRepository = paymentRepository;
    }

    public PaymentDto Query(Guid id)
    {
        // This class is kinda overkill but creating to demonstrate that some logic COULD live here

        var paymentEntity = this.paymentRepository.Get(id);

        // Definitely better to use Maybe pattern or something better for nulls but running out of time
        if (paymentEntity is null)
        {
            return null;
        }

        return new PaymentDto
        {
            Amount = paymentEntity.Amount,
            Currency = paymentEntity.Currency,
            Card = new CardDto
            {
                Cvv = paymentEntity.Card.Cvv,
                ExpiryMonth = paymentEntity.Card.ExpiryMonth,
                ExpiryYear = paymentEntity.Card.ExpiryYear,
                Number = paymentEntity.Card.MaskedNumber,
            },
        };
    }
}
