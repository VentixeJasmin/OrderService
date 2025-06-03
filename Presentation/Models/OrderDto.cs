using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class OrderDto
    //Lots of hep here from Claude AI with defining which fields the customer must enter.
{
    [Required]
    public string EventId { get; set; } = null!;

    public decimal PricePerTicket { get; set; }

    [Required]
    public int Quantity { get; set; } = 1;

    [Required]
    public int PaymentMethodId { get; set; }

    public List<PaymentMethodEntity> PaymentMethodOptions { get; set; } = [];

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [RequiredForPaymentMethod(2)]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [RequiredForPaymentMethod(3)]
    public string? StreetAddress { get; set; }

    [RequiredForPaymentMethod(3)]
    public string? PostCode { get; set; }

    [RequiredForPaymentMethod(3)]
    public string? City { get; set; }

    [RequiredForPaymentMethod(1)]
    [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Credit card number must be 13-19 digits")]
    [Display(Name = "Credit Card Number", Prompt = "1234 5678 9012 3456")]
    public string? CreditCardNumber { get; set; }
    
    [RequiredForPaymentMethod(1)]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Expiry must be in MM/YY format")]
    [Display(Name = "Expiry Date", Prompt = "MM/YY")]
    public string? Expires {  get; set; }

    [RequiredForPaymentMethod(1)]
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3-4 digits")]
    [Display(Name = "CVV", Prompt = "123")]
    public string? CVV { get; set; }
}
