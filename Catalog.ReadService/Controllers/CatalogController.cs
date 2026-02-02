using Microsoft.AspNetCore.Mvc;
using Catalog.ReadService.Data;
using MongoDB.Driver;

namespace Catalog.ReadService.Controllers;

[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly CatalogContext _context;

    public CatalogController(CatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _context.Items
            .Find(_ => true)
            .ToListAsync();

        return Ok(items);
    }


    [HttpPost("admin")]
    public async Task<IActionResult> AddItem(
    [FromHeader(Name = "X-ADMIN-KEY")] string adminKey,
    [FromBody] Models.CatalogItem item)
    {
        var isValidAdmin = await _context.AdminKeys
            .Find(k => k.Key == adminKey && k.IsActive)
            .AnyAsync();

        //if (!isValidAdmin)
          //  return Unauthorized("Invalid admin key");

        await _context.Items.InsertOneAsync(item);
        return Ok(item);
    }
    //[HttpPost("seed")]
    //public async Task<IActionResult> Seed()
    //{
    //    await _context.Items.InsertOneAsync(new Models.CatalogItem
    //    {
    //        Id = "1",
    //        Name = "Phone",
    //        Price = 30000,
    //        Stock = 25
    //    });
    //    return Ok("Inserted");
    //}


}