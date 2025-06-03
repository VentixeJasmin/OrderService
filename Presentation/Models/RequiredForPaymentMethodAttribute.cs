using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

//I had Claude AI generate this for me as a way of only requesting payment info for the chosen payment type
public class RequiredForPaymentMethodAttribute(int paymentMethodId) : ValidationAttribute
{
    private readonly int _paymentMethodId = paymentMethodId;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var orderDto = (OrderDto)validationContext.ObjectInstance;

        if (orderDto.PaymentMethodId == _paymentMethodId && string.IsNullOrEmpty(value?.ToString()))
        {
            return new ValidationResult($"This field is required for the selected payment method.");
        }

        return ValidationResult.Success;
    }
}
