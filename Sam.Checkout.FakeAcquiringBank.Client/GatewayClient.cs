using Sam.Checkout.Domain;
using System.Net.Http.Json;

namespace Sam.Checkout.FakeAcquiringBank.Client;
public class GatewayClient : IGatewayClient
{
    public async Task<PaymentResultDto> Post(HttpClient httpClient, PaymentDto paymentDto)
    {
        var response = await httpClient.PostAsJsonAsync("api/payment", paymentDto);
        response.EnsureSuccessStatusCode();
        var paymentResult = await response.Content.ReadFromJsonAsync<PaymentResultDto>();
        return paymentResult is null ? throw new Exception("Payment failed. Do something better") : paymentResult;
    }

    public async Task<PaymentDto> Get(HttpClient client, Guid id)
    {
        var response = await client.GetAsync($"api/Payment/{id}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PaymentDto>();
        return result is null ? throw new Exception("Payment Get failed. Do something better") : result;
    }
}
