using Microsoft.EntityFrameworkCore;
using Sam.Checkout.Domain;

namespace Sam.Checkout.Infrastructure;
public class PaymentRepository : IPaymentRepository
{
    private readonly GatewayDbContext context;

    public PaymentRepository(GatewayDbContext context)
    {
        this.context = context;
        this.context.Database.EnsureCreated(); // not prod ready
    }

    public void Create(PaymentEntity paymentEntity)
    {
        context.Payments.Add(paymentEntity);
        context.SaveChanges();
    }

    public PaymentEntity Get(Guid id)
    {
        return context.Payments
            .Include(p => p.Card)
            .SingleOrDefault(p => p.Id == id);
    }
}
