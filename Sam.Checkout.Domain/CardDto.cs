using System.ComponentModel.DataAnnotations;

namespace Sam.Checkout.Domain;

public class CardDto
{
    [Required]
    [StringLength(16)] // I know not all cards are 16 digits but keeping is simple
    public required string Number { get; set; }

    [Required]
    [MaxLength(2)]
    [MinLength(2)]
    public required int ExpiryMonth { get; set; }

    [Required]
    [MaxLength(2)]
    [MinLength(2)]
    public required int ExpiryYear { get; set; }

    [Required]
    [StringLength(3)]
    public required string Cvv { get; set; }
}
