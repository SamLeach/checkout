using Microsoft.AspNetCore.Mvc;

namespace Sam.Checkout.FakeAquiringBank;
[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    [HttpPost]
    public PaymentResponse Pay(PaymentRequest paymentRequest)
    {
        // Gateway can simulate a non success response from aquiring bank by sending cvv as 999.
        // Big assumption here is that 999 can never be a valid "real" cvv

        return new PaymentResponse { Id = Guid.NewGuid(), Success = true };
    }
}
