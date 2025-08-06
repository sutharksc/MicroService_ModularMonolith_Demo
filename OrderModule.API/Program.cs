using OrderModule.API.DI;
using OrderModule.Application;
using OrderModule.Infrastructure;
using SharedModule.DI;
using SharedModule.EndPoint;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.RegisterEndpointsFromAssemblies(typeof(OrderModuleIdentityRoot).Assembly);

// Access IConfiguration instance
IConfiguration configuration = builder.Configuration;
builder.Services.AddGrpc();

builder.Services.AddOrderModule(configuration);
builder.Services.ConfigureServices();
builder.Services.AddSharedModule();

var app = builder.Build();

app.UseSharedModule();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOrderModule();

app.MapMinimalEndpoints();

app.Run();
