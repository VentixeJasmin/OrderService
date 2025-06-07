using Data.Entities;

namespace Presentation.Models; 

public class OrderFormDataDto
{
    public List<PaymentMethodEntity> PaymentMethodOptions { get; set; } = [];
}
