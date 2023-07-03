namespace Sam.Checkout.Domain;
public interface IPaymentQuery
{
    PaymentDto Query(Guid id);
}
