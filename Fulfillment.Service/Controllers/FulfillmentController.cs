using Microsoft.AspNetCore.Mvc;
using Fulfillment.Service.Handlers;

namespace Fulfillment.Service.Controllers;

[ApiController]
[Route("api/fulfillment")]
public class FulfillmentController : ControllerBase
{
    private readonly CreateFulfillmentHandler _handler;

    public FulfillmentController(CreateFulfillmentHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Fulfill([FromBody] FulfillmentRequest request)
    {
        var id = await _handler.Handle(request.OrderId);
        return Ok(new { FulfillmentId = id });
    }
}

public class FulfillmentRequest
{
    public Guid OrderId { get; set; }
}