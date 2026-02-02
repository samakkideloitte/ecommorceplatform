using Microsoft.AspNetCore.Mvc;
using Payments.Service.Handlers;
using Payments.Service.Models;

namespace Payments.Service.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly CreatePaymentHandler _handler;

    public PaymentsController(CreatePaymentHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Pay([FromBody] PaymentRequest request)
    {
        var paymentId = await _handler.Handle(request.OrderId, request.Amount);
        return Ok(new { PaymentId = paymentId });
    }
}