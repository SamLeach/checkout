using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sam.Checkout.Domain;
using Sam.Checkout.FakeAcquiringBank.Client;
using Sam.Checkout.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentCommandHandler, PaymentCommandHandler>();
builder.Services.AddScoped<IPaymentQuery, PaymentQuery>();

builder.Services.AddScoped<IFakeBankClient, FakeBankClient>();
builder.Services.AddScoped<IGatewayClient, GatewayClient>();

builder.Services.AddScoped<IValidator<CardDto>, CardDtoValidator>();
builder.Services.AddScoped<IValidator<PaymentDto>, PaymentDtoValidator>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hard coding connection string here. It should be injected from config 
builder.Services.AddDbContext<GatewayDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Gateway;Trusted_Connection=True;MultipleActiveResultSets=true"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
