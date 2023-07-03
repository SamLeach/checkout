namespace Sam.Checkout.Domain;
public interface IPaymentRepository
{
    void Create(PaymentEntity paymentEntity);

    PaymentEntity Get(Guid id);
}
