using Microsoft.EntityFrameworkCore;
using Fulfillment.Service.Data;
using Fulfillment.Service.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<FulfillmentDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FulfillmentDbNew")));

builder.Services.AddScoped<CreateFulfillmentHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANT: no HTTPS redirect for local dev
// app.UseHttpsRedirection();

app.MapControllers();
app.Run();