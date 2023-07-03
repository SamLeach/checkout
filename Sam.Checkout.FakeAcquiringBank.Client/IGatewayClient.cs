using Sam.Checkout.Domain;

namespace Sam.Checkout.FakeAcquiringBank.Client;
public interface IGatewayClient
{
    Task<PaymentResultDto> Post(HttpClient httpClient, PaymentDto paymentDto);

    Task<PaymentDto> Get(HttpClient client, Guid id);
}
