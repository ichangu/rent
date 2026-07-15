using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PropertyManagerApp;
using PropertyManagerApp.Services; // Tells the app where to find your service

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Standard Blazor HTTP client setup
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// REGISTER YOUR DATABASE SERVICE HERE:
builder.Services.AddScoped<DatabaseService>();

await builder.Build().RunAsync();