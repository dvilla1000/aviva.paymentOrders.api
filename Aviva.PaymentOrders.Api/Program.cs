using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Aviva.PaymentOrders.Application.Adapters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddHttpClient<Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders.PagaFacilProvider>(client =>
{
    client.BaseAddress = new Uri("https://app-paga-chg-aviva.azurewebsites.net");
    client.DefaultRequestHeaders.Add("x-api-key", "apikey-1cnmoisyhkif3s");
});

builder.Services.AddHttpClient<Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders.CazaPagosProvider>(client =>
{
    client.BaseAddress = new Uri("https://app-caza-chg-aviva.azurewebsites.net");
    client.DefaultRequestHeaders.Add("x-api-key", "apikey-1cnmoisyhkif3s");
});

builder.Services.AddScoped<Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders.IPaymentProviderFactory, Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders.PaymentProviderFactory>();
builder.Services.AddScoped<Aviva.PaymentOrders.DataInfrastructure.Repositories.ProductsRepository>();
builder.Services.AddScoped<Aviva.PaymentOrders.Application.Services.ProductService>();
builder.Services.AddScoped<Aviva.PaymentOrders.DataInfrastructure.Repositories.OrdersRepository>();
builder.Services.AddScoped<Aviva.PaymentOrders.Application.Services.OrderService>();
builder.Services.AddScoped<Aviva.PaymentOrders.DataInfrastructure.Data.InMemoryContext>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
    
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    });
}

app.MapControllers();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
