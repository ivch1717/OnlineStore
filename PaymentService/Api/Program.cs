using Infrastructure;
using Presentation;
using UseCases.CreateAccount;
using UseCases.GetBalance;
using UseCases.TopUpAccount;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPaymentsInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICreateAccountRequestHandler, CreateAccountRequestHandler>();
builder.Services.AddScoped<IGetBalanceRequestHandler, GetBalanceRequestHandler>();
builder.Services.AddScoped<ITopUpAccountRequestHandler, TopUpAccountRequestHandler>();

Console.WriteLine(builder.Configuration.GetConnectionString("Default"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapAccountsEndpoints();

app.Run();