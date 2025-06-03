using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class PaymentMethodEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string PaymentMethod { get; set; } = null!;

    [Required]
    public bool DirectPayment { get; set; }
}