using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockShop.Application.Interfaces;
using MockShop.Domain.Entities;
using System.ComponentModel.DataAnnotations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IOrderRepository _orderRepository;
    private readonly IMessageProducer _messageProducer;

    public OrdersController(IPaymentService paymentService,
                            IOrderRepository orderRepository,
                            IMessageProducer messageProducer)
    {
        _paymentService = paymentService;
        _orderRepository = orderRepository;
        _messageProducer = messageProducer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        // 1. Calculate total amount
        decimal totalAmount = request.Items.Sum(x => x.Quantity * x.UnitPrice);

        // 2. Take payment
        bool paymentSuccess = await _paymentService.ProcessPaymentAsync(totalAmount, request.CardNumber);
        if (!paymentSuccess) return BadRequest("Ödeme reddedildi.");

        // 3. Create order item
        var newOrder = new Order
        {
            UserId = 1,
            OrderDate = DateTimeOffset.UtcNow,
            Status = "Paid",
            TotalAmount = totalAmount,
            Items = request.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }).ToList()
        };

        // 4. Save
        await _orderRepository.CreateAsync(newOrder);

        // 5. RabbitMQ
        _messageProducer.SendMessage(new { OrderId = newOrder.Id, Status = "Paid" });

        return Ok(new { Message = "Sipariş alındı.", OrderId = newOrder.Id });
    }
}

public class CreateOrderRequest
{
    public string CardNumber { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}