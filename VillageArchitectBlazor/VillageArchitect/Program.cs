using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor; // Required for classes like PaletteLight, PaletteDark, Typography, Default, etc.
using MudBlazor.Services; // For AddMudServices()
using VillageArchitect;
using VillageArchitect.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register MudBlazor services (no theme here)
builder.Services.AddMudServices(config =>
{
    // Optional: Snackbar config for notifications (e.g., errors)
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

// Register your custom services
builder.Services.AddSingleton<GeminiService>();

await builder.Build().RunAsync();