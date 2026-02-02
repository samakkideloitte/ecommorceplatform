using Microsoft.EntityFrameworkCore;
using Fulfillment.Service.Domain;
using System.Collections.Generic;

namespace Fulfillment.Service.Data;

public class FulfillmentDbContext : DbContext
{
    public FulfillmentDbContext(DbContextOptions<FulfillmentDbContext> options)
    : base(options) { }

    public DbSet<Fulfillment.Service.Domain.Fulfillment> Fulfillments { get; set; }
}
