using Sam.Checkout.Domain;

namespace Sam.Checkout.IntegrationTests;
public class FakeBankClientStub : IFakeBankClient
{
    public Task<PaymentResultDto> Post(PaymentDto paymentDto)
    {
        return Task.FromResult(new PaymentResultDto { Id = Guid.NewGuid(), Success = true });
    }
}
