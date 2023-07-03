using Microsoft.EntityFrameworkCore;
using Sam.Checkout.Domain;

namespace Sam.Checkout.Infrastructure;
public class GatewayDbContext : DbContext
{
    public GatewayDbContext(DbContextOptions<GatewayDbContext> options) : base(options)
    {
    }

    public DbSet<CardEntity> Cards { get; set; }

    public DbSet<PaymentEntity> Payments { get; set; }
}
