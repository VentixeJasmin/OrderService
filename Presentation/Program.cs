using Azure.Messaging.ServiceBus;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOrderRepository, OrderRepsitory>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Got help with the ServiceBus configuring by Claude AI
builder.Services.AddSingleton<ServiceBusClient>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("ServiceBus");
    return new ServiceBusClient(connectionString);
});

builder.Services.AddDbContext<OrderContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("VentixeDatabaseConnection")));

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
