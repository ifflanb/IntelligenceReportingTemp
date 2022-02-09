using System.Diagnostics;
using System.Globalization;
using IntelligenceReporting.WebApp;
using IntelligenceReporting.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.JSInterop;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

// Generically register all Services by their Interfaces
var interfacesAssembly = typeof(IApiService).Assembly;
foreach (var type in typeof(ApiService).Assembly.GetTypes())
{
    if (type.IsAbstract) continue;

    foreach (var interfaceType in type.GetInterfaces())
    {
        if (interfaceType.Assembly != interfacesAssembly) continue;

        builder.Services.AddScoped(interfaceType, type);
    }
}

// I18n Services
builder.Services
    .AddLocalization(options => options.ResourcesPath = "Resources")
    .Configure<RequestLocalizationOptions>(
        opts =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                // Set Supported Locales
                new("en-AU"),
                new("en-NZ"),
                new("en-ID")
            };

            opts.DefaultRequestCulture = new RequestCulture("en-AU");
            // Formatting numbers, dates, etc.
            opts.SupportedCultures = supportedCultures;
            // UI strings that we have localized.
            opts.SupportedUICultures = supportedCultures;
        }
    );


var host = builder.Build();
var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
var appCulture = await jsInterop.InvokeAsync<string>("appCulture.get");
if (!string.IsNullOrEmpty(appCulture))
{
    var culture = new CultureInfo(appCulture);
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

// Get static data on startup and save to local storage
var apiService = host.Services.GetRequiredService<IApiService>();
var staticData = await apiService.GetStaticData();
var localStorageService = host.Services.GetRequiredService<ILocalStorageService>();
await localStorageService.Set(staticData);

await builder.Build().RunAsync();
