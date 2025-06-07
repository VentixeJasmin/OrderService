using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Interfaces;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(ICustomerService customerService, IOrderService orderService, IPaymentMethodRepository paymentMethodRepository) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly IOrderService _orderService = orderService;
        private readonly IPaymentMethodRepository _paymentMethodRepository = paymentMethodRepository;

        [HttpGet("form-data")]
        public async Task<IActionResult> GetOrderFormData()
        {
            try
            {
                OrderFormDataDto dto = new();
                dto.PaymentMethodOptions = await PopulatePaymentMethodsAsync();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = "Couldn't load payment options."});
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(error: "Form does not contain all required values.");
            }

            try
            {
                var customerResult = await _customerService.CreateCustomer(dto);
                if (customerResult == null)
                {
                    return BadRequest( error: "Error creating customer."); 
                }

                var orderResult = await _orderService.CreateOrder(dto, customerResult.Id);
                if (orderResult == null)
                {
                    return BadRequest(error: "Error creating order.");
                }

                return Created("", orderResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrders(string? status)
        {
            var orders = await _orderService.GetAllOrders();

            if (orders != null)
            {
                if (status != null && status.ToLowerInvariant().Equals("paid"))
                {
                    var paidOrders = orders
                        .Where(o => o.IsPaid == true)
                        .ToList();

                    return Ok(paidOrders);
                }
                else if (status != null && status.ToLowerInvariant().Equals("unpaid"))
                {
                    var unpaidOrders = orders
                        .Where(o => o.IsPaid == false)
                        .ToList();

                    return Ok(unpaidOrders);
                }
                else
                {
                    return Ok(orders);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var entity = await _orderService.GetOrderById(id);

            if (entity != null)
            {
                return Ok(entity);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<List<PaymentMethodEntity>> PopulatePaymentMethodsAsync()
        {
            var paymentMethods = await _paymentMethodRepository.GetAsync();
            List<PaymentMethodEntity> options = [];

            foreach (PaymentMethodEntity method in paymentMethods)
            {
                options.Add(method);
            }

            return options;
        }
    }
}
