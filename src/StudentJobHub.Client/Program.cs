using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudentJobHub.Client;
using StudentJobHub.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5205/")
});

builder.Services.AddScoped<JobApiService>();
builder.Services.AddScoped<ApplicationApiService>();

await builder.Build().RunAsync();