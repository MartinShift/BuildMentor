using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddDbContext<BuildContext>((options) =>
{
    SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
    var path = "./BuildDb.db";
    options.UseSqlite($"Data Source={path}");

});
var configuration = builder.Configuration;
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
            new CultureInfo("en-US"),
            new CultureInfo("uk-UA"),
        };
    options.DefaultRequestCulture = new RequestCulture("uk-UA");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


builder.Services.AddControllersWithViews()
     .AddViewLocalization();

builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<InstructionService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ToolMaintenanceService>();
builder.Services.AddScoped<ToolPermissionRequestService>();
builder.Services.AddScoped<ToolPermissionService>();
builder.Services.AddScoped<ToolService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SmtpService>();
builder.Services.AddScoped<UserToolService>();
builder.Services.AddScoped<ActiveMaintenanceService>();
builder.Services.AddScoped<AdminRequestService>();
builder.Services.AddScoped<UserNotificationService>();
builder.Services.AddScoped<UnitService>();
builder.Services.AddScoped<DataSeed>();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<BuildContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});
var app = builder.Build();

app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.Use(async (context, next) =>
{
    var selectedLanguage = context.Request.Cookies["SelectedLanguage"];

    if (!string.IsNullOrEmpty(selectedLanguage))
    {
        CultureInfo.CurrentCulture = new CultureInfo(selectedLanguage);
        CultureInfo.CurrentUICulture = new CultureInfo(selectedLanguage);
    }

    await next();
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            var logger = app.Logger;
            logger.LogError($"Something went wrong: {contextFeature.Error}");

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error."
            }));
        }
    });
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}");

});


app.Run();