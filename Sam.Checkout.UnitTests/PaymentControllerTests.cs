using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sam.Checkout.Domain;
using Xunit;

namespace Sam.Checkout.UnitTests;

public class PaymentControllerTests
{
    private readonly Mock<IPaymentCommandHandler> paymentCommandHandlerMock;
    private readonly Mock<IPaymentQuery> paymentQueryMock;
    private readonly IValidator<PaymentDto> paymentValidator;

    public PaymentControllerTests()
    {
        paymentCommandHandlerMock = new Mock<IPaymentCommandHandler>();
        paymentQueryMock = new Mock<IPaymentQuery>();
        paymentValidator = new PaymentDtoValidator();
    }

    [Fact]
    public async Task Post_ValidPayload_OK200()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        Guid paymentId = Guid.NewGuid();
        var dto = new PaymentDto
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

        paymentCommandHandlerMock
            .Setup(c => c.Handle(dto))
            .ReturnsAsync(new PaymentResultDto { Id = paymentId, Success = true });

        var postResponse = await paymentController.Post(dto);

        var okResult = Assert.IsType<OkObjectResult>(postResponse);
        var paymentResultDto = Assert.IsType<PaymentResultDto>(okResult.Value);
        Assert.True(paymentResultDto.Success);
    }

    [Fact]
    public async Task Post_PaymentFailed_OK200()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        Guid paymentId = Guid.NewGuid();
        var dto = new PaymentDto
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

        paymentCommandHandlerMock
            .Setup(c => c.Handle(dto))
            .ReturnsAsync(new PaymentResultDto { Id = paymentId, Success = false });

        var postResponse = await paymentController.Post(dto);

        var okResult = Assert.IsType<OkObjectResult>(postResponse);
        var paymentResultDto = Assert.IsType<PaymentResultDto>(okResult.Value);
        Assert.False(paymentResultDto.Success);
    }

    [Fact]
    public async Task Post_InvalidCurrency_BadRequest400()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var dto = new PaymentDto
        {
            Amount = 123,
            Currency = "P",
            Card = new CardDto
            {
                Cvv = "345",
                ExpiryMonth = 12,
                ExpiryYear = 99,
                Number = "1234123412341234"
            }
        };

        var postResponse = await paymentController.Post(dto);

        Assert.IsType<BadRequestResult>(postResponse);
    }

    [Fact]
    public async Task Post_InvalidNegativeAmount_BadRequest400()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var dto = new PaymentDto
        {
            Amount = -1,
            Currency = "GBP",
            Card = new CardDto
            {
                Cvv = "345",
                ExpiryMonth = 12,
                ExpiryYear = 99,
                Number = "1234123412341234"
            }
        };

        var postResponse = await paymentController.Post(dto);

        Assert.IsType<BadRequestResult>(postResponse);
    }

    [Fact]
    public async Task Post_InvalidCvv_BadRequest400()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var dto = new PaymentDto
        {
            Amount = -1,
            Currency = "GBP",
            Card = new CardDto
            {
                Cvv = "3",
                ExpiryMonth = 12,
                ExpiryYear = 99,
                Number = "1234123412341234"
            }
        };

        var postResponse = await paymentController.Post(dto);

        Assert.IsType<BadRequestResult>(postResponse);
    }

    [Fact]
    public async Task Post_InvalidCardNumber_BadRequest400()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var dto = new PaymentDto
        {
            Amount = -1,
            Currency = "GBP",
            Card = new CardDto
            {
                Cvv = "3",
                ExpiryMonth = 12,
                ExpiryYear = 99,
                Number = "123"
            }
        };

        var postResponse = await paymentController.Post(dto);

        Assert.IsType<BadRequestResult>(postResponse);
    }

    [Fact]
    public async Task Post_InvalidExpiryMonth_BadRequest400()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var dto = new PaymentDto
        {
            Amount = -1,
            Currency = "GBP",
            Card = new CardDto
            {
                Cvv = "3",
                ExpiryMonth = 122,
                ExpiryYear = 99,
                Number = "123"
            }
        };

        var postResponse = await paymentController.Post(dto);

        Assert.IsType<BadRequestResult>(postResponse);
    }

    [Fact]
    public async Task Post_InvalidExpiryYear_BadRequest400()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var dto = new PaymentDto
        {
            Amount = -1,
            Currency = "GBP",
            Card = new CardDto
            {
                Cvv = "3",
                ExpiryMonth = 12,
                ExpiryYear = 200,
                Number = "123"
            }
        };

        var postResponse = await paymentController.Post(dto);

        Assert.IsType<BadRequestResult>(postResponse);
    }

    [Fact]
    public void Get_ValidPaymentId_OK200()
    {
        Guid queryId = Guid.NewGuid();

        var payment = new PaymentDto
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

        paymentQueryMock
            .Setup(m => m.Query(queryId))
            .Returns(payment);

        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var getResponse = paymentController.Get(queryId);

        var okResult = Assert.IsType<OkObjectResult>(getResponse);
        var paymentDto = Assert.IsType<PaymentDto>(okResult.Value);

        Assert.Equal(payment.Amount, paymentDto.Amount);
        Assert.Equal(payment.Currency, paymentDto.Currency);
        Assert.Equal(payment.Card.Cvv, paymentDto.Card.Cvv);
        Assert.Equal(payment.Card.ExpiryMonth, paymentDto.Card.ExpiryMonth);
        Assert.Equal(payment.Card.ExpiryYear, paymentDto.Card.ExpiryYear);
        Assert.Equal(payment.Card.Number, paymentDto.Card.Number);
    }

    [Fact]
    public void Get_NoMatchingPayment_NotFound200()
    {
        Guid queryId = Guid.NewGuid();

        paymentQueryMock
            .Setup(m => m.Query(queryId))
            .Returns((PaymentDto)null);

        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var getResponse = paymentController.Get(queryId);

        var okResult = Assert.IsType<NotFoundResult>(getResponse);
    }

    [Fact]
    public void Get_InValidPaymentId_BadRequest400()
    {
        var paymentController = new Payment.PaymentController(paymentValidator, paymentCommandHandlerMock.Object, paymentQueryMock.Object);

        var getResponse = paymentController.Get(Guid.Empty);

        Assert.IsType<BadRequestResult>(getResponse);
    }
}