using SharedModule.DI;
using SharedModule.EndPoint;
using UserModule.API.DI;
using UserModule.Application;
using UserModule.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.RegisterEndpointsFromAssemblies(typeof(UserModuleIdentityRoot).Assembly);

// Access IConfiguration instance
IConfiguration configuration = builder.Configuration;
builder.Services.AddGrpc();

builder.Services.AddUserModule(configuration);
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
app.UseUserModule();

app.MapMinimalEndpoints();

app.Run();
