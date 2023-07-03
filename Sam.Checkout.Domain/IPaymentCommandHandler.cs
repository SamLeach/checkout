namespace Sam.Checkout.Domain;
public interface IPaymentCommandHandler
{
    Task<PaymentResultDto> Handle(PaymentDto paymentDto);
}
