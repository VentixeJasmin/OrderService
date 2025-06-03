using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;
public class OrderEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string EventId { get; set; } = null!;

    [Required]
    public DateTime OrderPlaced { get; set; } = DateTime.Now;

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal PricePerTicket { get; set; }

    [NotMapped]
    public decimal TotalPrice => Quantity * PricePerTicket;

    [Required]
    public int PaymentMethodId { get; set; }

    public PaymentMethodEntity PaymentMethod { get; set; } = null!; 

    [Required]
    public bool IsPaid { get; set; }

    [Required]
    public int CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!; 

}
