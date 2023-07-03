using Sam.Checkout.Domain;
using System.Net.Http.Json;

namespace Sam.Checkout.FakeAcquiringBank.Client;

public class FakeBankClient : IFakeBankClient
{
    private static HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:7232"), // TODO: Should be in config
    };

    public async Task<PaymentResultDto> Post(PaymentDto paymentDto)
    {
        var response = await httpClient.PostAsJsonAsync("api/payment", paymentDto);
        response.EnsureSuccessStatusCode();
        var paymentResult = await response.Content.ReadFromJsonAsync<PaymentResultDto>();
        return paymentResult is null ? throw new Exception("Payment failed. Do something better") : paymentResult;
    }
}
