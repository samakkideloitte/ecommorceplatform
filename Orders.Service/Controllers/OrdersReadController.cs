using Microsoft.AspNetCore.Mvc;
using Orders.Service.Queries;

namespace Orders.Service.Controllers;

[ApiController]
[Route("api/orders/read")]
public class OrdersReadController : ControllerBase
{
    private readonly OrderQueries _queries;

    public OrdersReadController(OrderQueries queries)
    {
        _queries = queries;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _queries.GetAll();
        return Ok(result);
    }
}