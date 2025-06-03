using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class CustomerEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? StreetAddress { get; set; } 

    public string? PostCode { get; set; }

    public string? City { get; set; }

    public string? PaymentTransactionId { get; set; } // From payment processor
    public string? LastFourDigits { get; set; } // For display: "****-1234"

    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();

}