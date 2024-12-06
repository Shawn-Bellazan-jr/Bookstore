using System.Diagnostics.Metrics;
using Bookstore.ApiService.Interfaces;
using Bookstore.ApiService.Services;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

try
{
    var redisConfig = new ConfigurationOptions
    {
        EndPoints = { "localhost:6379" }, // Fixed port
        AbortOnConnectFail = false,
        ConnectTimeout = 10000
    };

    var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConfig);
    builder.Services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
    builder.Services.AddScoped<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
}
catch (Exception ex)
{
    Console.WriteLine($"Redis connection failed: {ex.Message}");
}

// Add services to the container.
builder.Services.AddControllers(); // Add this line to enable MVC
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IBookService, BookService>();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bookstore API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Add this line to map controllers

app.Run();