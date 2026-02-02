using Microsoft.EntityFrameworkCore;
using Orders.Service.Background;
using Orders.Service.Data;
using Orders.Service.Handlers;
using Orders.Service.Queries;
using Orders.Service.Background;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrdersDbNew")));

builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<OrderQueries>();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

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
app.UseCors("AllowAll");
//app.UseHttpsRedirection();
app.MapControllers();

app.Run();