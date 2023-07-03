using Moq;
using Sam.Checkout.Domain;
using Sam.Checkout.FakeAcquiringBank.Client;
using Xunit;

namespace Sam.Checkout.UnitTests;
public class PaymentCommandHandlerTests
{
    private readonly Mock<IPaymentRepository> paymentRepository;
    private readonly Mock<IFakeBankClient> fakeBankClient;

    public PaymentCommandHandlerTests()
    {
        paymentRepository = new Mock<IPaymentRepository>();
        fakeBankClient = new Mock<IFakeBankClient>();
    }

    [Fact]
    public async Task Get_ValidPayment_ReturnsPaymentWithMaskedCardNumber()
    {
        var paymentCommandHandler = new PaymentCommandHandler(paymentRepository.Object, fakeBankClient.Object);

        var paymentDto = new PaymentDto 
        {
            Amount = 123,
            Currency = "GBP",
            Card = new CardDto
            {
                Cvv = "345",
                ExpiryMonth = 12,
                ExpiryYear = 99,
                Number = "1234123412341234"
            }
        };

        var paymentResultDto = new PaymentResultDto 
        {
            Id = Guid.NewGuid(),
            Success = true
        };

        fakeBankClient.Setup(c => c.Post(paymentDto)).ReturnsAsync(paymentResultDto);

        var paymentResult = await paymentCommandHandler.Handle(paymentDto);

        paymentRepository.Verify(r => r.Create(It.Is<PaymentEntity>(e => 
            e.Card.MaskedNumber == "1234############" && 
            e.AquiringBankPaymentId == paymentResultDto.Id)), Times.Once);

        fakeBankClient.Verify(r => r.Post(It.IsAny<PaymentDto>()), Times.Once);

        Assert.True(paymentResult.Success);
    }

    [Fact]
    public async Task Get_BankPaymentFails_ReturnsPaymentWithMaskedCardNumber()
    {
        var paymentCommandHandler = new PaymentCommandHandler(paymentRepository.Object, fakeBankClient.Object);

        var paymentDto = new PaymentDto
        {
            Amount = 123,
            Currency = "GBP",
            Card = new CardDto
            {
                Cvv = "345",
                ExpiryMonth = 12,
                ExpiryYear = 99,
                Number = "1234123412341234"
            }
        };

        var paymentResultDto = new PaymentResultDto
        {
            Id = Guid.NewGuid(),
            Success = false
        };

        fakeBankClient.Setup(c => c.Post(paymentDto)).ReturnsAsync(paymentResultDto);

        var paymentResult = await paymentCommandHandler.Handle(paymentDto);

        paymentRepository.Verify(r => r.Create(It.Is<PaymentEntity>(e =>
            e.Card.MaskedNumber == "1234############")), Times.Once);

        fakeBankClient.Verify(r => r.Post(It.IsAny<PaymentDto>()), Times.Once);

        Assert.False(paymentResult.Success);
    }
}
