using Microsoft.AspNetCore.Mvc;
using Orders.Service.Commands;
using Orders.Service.Handlers;

namespace Orders.Service.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _handler;

    public OrdersController(CreateOrderHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateOrderCommand command,
    [FromHeader(Name = "Idempotency-Key")] string idemKey)
    {
        var result = await _handler.Handle(command, idemKey);

        if (result == null)
            return Conflict("Duplicate request");

        return Ok(new { OrderId = result });
    }
}