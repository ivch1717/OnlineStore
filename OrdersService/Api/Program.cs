using Infrastructure;
using Presentation;
using UseCases.CreateOrder;
using UseCases.GetOrderById;
using UseCases.GetOrdersByUser;
using Infrastructure.Messaging;

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

app.UseSwagger();
app.UseSwaggerUI();

app.MapOrdersEndpoints();

app.Run();