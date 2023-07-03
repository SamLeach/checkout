namespace Sam.Checkout.Domain;
public interface IFakeBankClient
{
    Task<PaymentResultDto> Post(PaymentDto paymentDto);
}
