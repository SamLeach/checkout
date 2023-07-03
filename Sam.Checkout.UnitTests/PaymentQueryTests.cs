using Moq;
using Sam.Checkout.Domain;
using Xunit;

namespace Sam.Checkout.UnitTests;
public class PaymentQueryTests
{
    private readonly Mock<IPaymentRepository> paymentRepository;

    public PaymentQueryTests()
    {
        this.paymentRepository = new Mock<IPaymentRepository>();
    }

    [Fact]
    public void Get_ValidPayment_ReturnsPaymentWithMaskedCardNumber()
    {
        var queryId = Guid.NewGuid();

        var paymentEntity = new PaymentEntity
        {
            Amount = 123,
            Currency = "GBP",
            Card = new CardEntity
            {
                Cvv = "345",
                ExpiryMonth = 12,
                ExpiryYear = 99,
                MaskedNumber = "1234############"
            }
        };

        this.paymentRepository
            .Setup(r => r.Get(queryId))
            .Returns(paymentEntity);

        var paymentQuery = new PaymentQuery(this.paymentRepository.Object);

        var payment = paymentQuery.Query(queryId);

        Assert.Equal(paymentEntity.Amount, payment.Amount);
        Assert.Equal(paymentEntity.Currency, payment.Currency);
        Assert.Equal(paymentEntity.Card.Cvv, payment.Card.Cvv);
        Assert.Equal(paymentEntity.Card.ExpiryMonth, payment.Card.ExpiryMonth);
        Assert.Equal(paymentEntity.Card.ExpiryYear, payment.Card.ExpiryYear);
        Assert.Equal(paymentEntity.Card.MaskedNumber, payment.Card.Number);
    }

    [Fact]
    public void Get_NotFound_ReturnsNull()
    {
        var queryId = Guid.NewGuid();

        var paymentEntity = new PaymentEntity
        {
            Amount = 123,
            Currency = "GBP",
            Card = new CardEntity
            {
                Cvv = "345",
                ExpiryMonth = 12,
                ExpiryYear = 99,
                MaskedNumber = "1234############"
            }
        };

        this.paymentRepository
            .Setup(r => r.Get(queryId))
            .Returns((PaymentEntity)null);

        var paymentQuery = new PaymentQuery(this.paymentRepository.Object);

        var payment = paymentQuery.Query(queryId);

        Assert.Null(payment);
    }
}
