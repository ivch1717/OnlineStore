using Infrastructure;
using Infrastructure.Data.Db;
using Presentation;
using UseCases.CreateOrder;
using UseCases.GetOrderById;
using UseCases.GetOrdersByUser;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddHostedService<OrdersOutboxPublisherHostedService>();
builder.Services.AddHostedService<OrdersPaymentResultConsumerHostedService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOrdersInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICreateOrderRequestHandler, CreateOrderRequestHandler>();
builder.Services.AddScoped<IGetOrderByIdRequestHandler, GetOrderByIdRequestHandler>();
builder.Services.AddScoped<IGetOrdersByUserRequestHandler, GetOrdersByUserRequestHandler>();

Console.WriteLine(builder.Configuration.GetConnectionString("Default"));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderServiceDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapOrdersEndpoints();

app.Run();