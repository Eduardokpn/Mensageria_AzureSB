using MensageriaProject.Interfaces.Repository;
using MensageriaProject.Interfaces;
using MensageriaProject.Services;
using Azure.Messaging.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ Configuração do Azure Service Bus
builder.Services.AddSingleton<ServiceBusClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration["AzureServiceBus:ConnectionString"];
    return new ServiceBusClient(connectionString);
});

// ✅ Registro do ServiceBusService e HostedService
builder.Services.AddScoped<ServiceBusService>();

// ✅ Registro do Repositório
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


// ✅ Registro do Serviço de Email 
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
