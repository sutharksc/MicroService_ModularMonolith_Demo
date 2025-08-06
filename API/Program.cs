using API.DI;
using BookingModule.Application;
using BookingModule.Infrastructure;
using OrderModule.Application;
using OrderModule.Infrastructure;
using SharedModule.Abstractions;
using SharedModule.DI;
using SharedModule.EndPoint;
using UserModule.Application;
using UserModule.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.RegisterEndpointsFromAssemblies(typeof(BookingModuleIdentityRoot).Assembly,
    typeof(OrderModuleIdentityRoot).Assembly, typeof(UserModuleIdentityRoot).Assembly, typeof(SharedModuleIdentityRoot).Assembly);

// Access IConfiguration instance
IConfiguration configuration = builder.Configuration;

// Register gRPC service
builder.Services.AddGrpc();

builder.Services.AddBookingModule(configuration);
builder.Services.AddOrderModule(configuration);
builder.Services.AddUserModule(configuration);

builder.Services.ConfigureServices();

builder.Services.AddSharedModule();


var app = builder.Build();

app.UseSharedModule();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseOrderModule();
app.UseBookingModule();
app.UseUserModule();
app.MapMinimalEndpoints();


app.MapGet("/", (IUserContext userContext) =>
{
    return Results.Ok($"User Name : {userContext.UserName} UserId : {userContext.UserId}");
}).RequireAuthorization();


app.Run();

