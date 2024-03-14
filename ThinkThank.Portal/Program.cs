using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using ThinkThank.Portal;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var culturies = new[]
    {
        new CultureInfo("ru-Ru"),
        new CultureInfo("kk-Kz"),
        new CultureInfo("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture("ru-Ru", "ru-Ru");

    options.SupportedCultures = culturies;
    options.SupportedUICultures = culturies;
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services
    .AddMvc()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    }).AddViewLocalization();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions!.Value);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?statusCode={0}");
app.Run();