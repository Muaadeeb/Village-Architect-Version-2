using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services; // For AddMudServices()
using VillageArchitect;
using VillageArchitect.Services; // Your namespace for GeminiService

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app"); // Default
builder.RootComponents.Add<HeadOutlet>("head::after"); // Default

// Register MudBlazor services (critical)
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
builder.Services.AddSingleton<GeminiService>(); // For AI calls

// Optional: Custom theme for gritty fantasy (add if not present)
var customTheme = new MudTheme
{
    PaletteLight = new PaletteLight
    {
        Primary = Colors.Amber.Darken2, // "Manifest" button
        Secondary = Colors.Brown.Darken4, // Accents
        Background = Colors.Grey.Lighten4,
        AppbarBackground = Colors.Amber.Darken4,
        TextPrimary = Colors.Shades.Black,
        Error = Colors.Red.Darken4 // For "Pariah" styles, etc.
    },
    PaletteDark = new PaletteDark // Dark mode for Shadowdark vibe
    {
        Primary = Colors.Amber.Lighten1,
        Secondary = Colors.Brown.Lighten2,
        Background = Colors.Grey.Darken4,
        AppbarBackground = Colors.Amber.Darken3,
        TextPrimary = Colors.Shades.White
    },
    Typography = new Typography // Optional font tweaks
    {
        Default = new Default { FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" } }
    }
};
builder.Services.AddMudServices(config => { config.Theme = customTheme; }); // Override if needed

await builder.Build().RunAsync();