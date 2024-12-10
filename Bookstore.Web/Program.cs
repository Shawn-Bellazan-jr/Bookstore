using Bookstore.ServiceDefaults;
using Bookstore.Web;
using Bookstore.Web.Components;
using Bookstore.Web.Interfaces;
using Bookstore.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// Configure HttpClient with Aspire-compatible base URL.
builder.Services.AddHttpClient<IUnitOfWork>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://apiservice");
});

// Register UnitOfWork and GenericService.
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
//builder.Services.AddScoped<IBookService, BookService>();
//builder.Services.AddScoped<ICustomerService, CustomerService>();
//builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
