using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Sam.Checkout.Domain;
using Sam.Checkout.FakeAcquiringBank.Client;
using Xunit;

namespace Sam.Checkout.IntegrationTests;

public class PaymentTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IGatewayClient _gatewayClient;

    public PaymentTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _gatewayClient = new GatewayClient();
    }

    [Fact]
    public async Task Payment_ValidPayment_InsertsAndReturnsSuccessfulPayment()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<IFakeBankClient, FakeBankClientStub>();
            });
        }).CreateClient();

        var paymentDto = new PaymentDto
        { 
            Amount = 123,
            Currency = "GBP",
            Card = new CardDto
            { 
                Number = "1234123412341234",
                Cvv = "123",
                ExpiryMonth = 10,
                ExpiryYear = 23
            } 
        };

        // Act
        var paymentResult = await _gatewayClient.Post(client, paymentDto);

        // Assert
        Assert.NotNull(paymentResult);
        Assert.True(paymentResult.Success);
        Assert.NotEqual(paymentResult.Id, Guid.Empty);

        var getPaymentResult = await _gatewayClient.Get(client, paymentResult.Id);

        Assert.NotNull(getPaymentResult);

        Assert.Equal(paymentDto.Amount, getPaymentResult.Amount);
        Assert.Equal(paymentDto.Currency, getPaymentResult.Currency);
        Assert.Equal(paymentDto.Card.Cvv, getPaymentResult.Card.Cvv);
        Assert.Equal(paymentDto.Card.ExpiryMonth, getPaymentResult.Card.ExpiryMonth);
        Assert.Equal(paymentDto.Card.ExpiryYear, getPaymentResult.Card.ExpiryYear);
        Assert.Equal("1234############", getPaymentResult.Card.Number);

        //TODO: Delete the records that the tests create or recreate the db. Not doing this to save time.
    }
}
