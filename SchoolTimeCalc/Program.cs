using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using Refit;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<MockAuthService>();
builder.Services.AddScoped<WebUntisService>();
builder.Services.AddScoped<INationalHolidayService, NationalHolidayService>();
builder.Services.AddScoped<ISchoolHolidayService, SchoolHolidayService>();
builder.Services.AddScoped<IHolidaySyncService, WebUntisHolidaySyncService>();
builder.Services.AddHostedService<HolidaySyncBackgroundService>();

builder.Services.AddHttpClient("WebUntis")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddHttpClient("DataGvAt")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Automatically apply migrations and seed mock user data on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAntiforgery();

// Ensure static files can be served
app.UseStaticFiles();

app.MapRazorComponents<SchoolTimeCalc.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();