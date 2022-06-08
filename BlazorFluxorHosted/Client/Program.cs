global using BlazorFluxorHosted.Client;
global using Fluxor;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using System.Net.Http.Json;
global using BlazorFluxorHosted.Shared;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluxor(options => options.ScanAssemblies(typeof(Program).Assembly).UseReduxDevTools());

await builder.Build().RunAsync();
