using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sam.Checkout.Domain;

namespace Sam.Checkout.Payment;
[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IValidator<PaymentDto> paymentValidator;
    private readonly IPaymentCommandHandler paymentHandler;
    private readonly IPaymentQuery paymentQuery;

    public PaymentController(IValidator<PaymentDto> paymentValidtor, 
        IPaymentCommandHandler paymentHandler, 
        IPaymentQuery paymentQuery)
    {
        this.paymentValidator = paymentValidtor;
        this.paymentHandler = paymentHandler;
        this.paymentQuery = paymentQuery;
    }

    [HttpPost]
    public async Task<IActionResult> Post(PaymentDto paymentDto)
    {
        // Can put in action filter but only a couple actions here so leaving it here
        var validationResult = await this.paymentValidator.ValidateAsync(paymentDto);
        if (!validationResult.IsValid)
        {
            return BadRequest();
        }

        // This could use MediatR or MassTransit or something similar to decouple but keeping it simple hard method call for this exercise
        // Please also imagine that the Dtos live in this web layer and are mapped to Domain Entities from here. I could do this but not worth 
        // the time for an interview exercise. AutoMapper is nice.
        var paymentResult = await this.paymentHandler.Handle(paymentDto);

        return Ok(paymentResult);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest();
        }

        var paymentEntity = this.paymentQuery.Query(id);
        return paymentEntity is null ? NotFound() : Ok(paymentEntity);
    }
}
