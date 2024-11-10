using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options => {
    var supportedCultures = new[] { CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo("en"), CultureInfo.GetCultureInfo("fr") };
    var supportedUICultures = new[] { CultureInfo.GetCultureInfo("en"), CultureInfo.GetCultureInfo("fr") };

    options.DefaultRequestCulture = new(supportedUICultures[0]);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedUICultures;

    options.RequestCultureProviders = new[]
    {
        new UserRequestCultureProvider()
    };
});

var app = builder.Build();

app.UseRequestLocalization();

app.MapGet("/culture", () => {
    return new
    {
        DotNetVersion = Environment.Version,
        CurrentCulture = CultureInfo.CurrentCulture.Name,
        CurrentUICulture = CultureInfo.CurrentUICulture.Name,
    };
});

app.Run();

internal class UserRequestCultureProvider : IRequestCultureProvider
{
    public Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var result = new ProviderCultureResult(culture: CultureInfo.InvariantCulture.Name, uiCulture: "en");

        return Task.FromResult<ProviderCultureResult?>(result);
    }
}
