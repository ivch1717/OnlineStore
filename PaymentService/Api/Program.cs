using Infrastructure;
using Infrastructure.Data.Db;
using Presentation;
using UseCases.CreateAccount;
using UseCases.GetBalance;
using UseCases.TopUpAccount;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddHostedService<PaymentsPaymentRequestedConsumerHostedService>();
builder.Services.AddHostedService<PaymentsOutboxPublisherHostedService>();

builder.Services.AddPaymentsInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICreateAccountRequestHandler, CreateAccountRequestHandler>();
builder.Services.AddScoped<IGetBalanceRequestHandler, GetBalanceRequestHandler>();
builder.Services.AddScoped<ITopUpAccountRequestHandler, TopUpAccountRequestHandler>();

Console.WriteLine(builder.Configuration.GetConnectionString("Default"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentServiceDbContext>();
    db.Database.Migrate();
}


app.UseSwagger();
app.UseSwaggerUI();

app.MapAccountsEndpoints();

app.Run();