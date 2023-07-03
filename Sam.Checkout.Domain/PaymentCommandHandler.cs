namespace Sam.Checkout.Domain;

public class PaymentCommandHandler : IPaymentCommandHandler
{
    private readonly IPaymentRepository paymentRepository;
    private readonly IFakeBankClient fakeBankClient;

    public PaymentCommandHandler(IPaymentRepository paymentRepository, IFakeBankClient fakeBankClient)
    {
        this.paymentRepository = paymentRepository;
        this.fakeBankClient = fakeBankClient;
    }

    public async Task<PaymentResultDto> Handle(PaymentDto paymentDto)
    {
        var paymentResult = await this.fakeBankClient.Post(paymentDto);

        var bankResponse = new AquiringBankPaymentResponse { Id = paymentResult.Id, Success = paymentResult.Success };

        // Mapped here. This could be elsewhere but felt there are already too many abstractions for a simple exercise
        var paymentEntity = new PaymentEntity
        {
            AquiringBankPaymentId = bankResponse.Id,
            Status = 0,
            Card = new CardEntity
            {
                MaskedNumber = paymentDto.Card.Number[..4] + "############",
                Cvv = paymentDto.Card.Cvv,
                ExpiryMonth = paymentDto.Card.ExpiryMonth,
                ExpiryYear = paymentDto.Card.ExpiryYear
            }
        };

        this.paymentRepository.Create(paymentEntity);

        // Retuning our internal gateway id and not the acquiring bank id. We store it in the db however so we can lookup either
        return new PaymentResultDto { Id = paymentEntity.Id, Success = bankResponse.Success };
    }
}
