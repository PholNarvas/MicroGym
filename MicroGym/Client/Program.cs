using MicroGym.Client;
using MicroGym.Client.Auth;
using MicroGym.Client.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Default HttpClient (for public endpoints like login/register)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Auth
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthStateProvider>());
builder.Services.AddAuthorizationCore();

// JWT message handler — automatically attaches Bearer token to requests
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Authenticated HttpClient for services that call protected API endpoints
builder.Services.AddHttpClient<MemberService>(client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient<AttendanceClientService>(client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient<MembershipTypeClientService>(client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

// AntDesign
builder.Services.AddAntDesign();

// Client Services
builder.Services.AddScoped<AuthClientService>();
builder.Services.AddScoped<RevenueService>();
builder.Services.AddScoped<PaymentClientService>();
builder.Services.AddScoped<MicroGym.Client.Services.ModalService>();
builder.Services.AddSingleton<MicroGym.Client.Services.ThemeService>();

await builder.Build().RunAsync();
