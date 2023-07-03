using System.ComponentModel.DataAnnotations;

namespace Sam.Checkout.Domain;

public class PaymentDto
{
    [Required]
    public required CardDto Card { get; set; }

    [Required]
    public required decimal Amount { get; set; }

    [Required]
    [StringLength(3)]
    // I could create a custom validator or Enum, etc. But using a string due to time constrainsts
    public required string Currency { get; set; }
}
