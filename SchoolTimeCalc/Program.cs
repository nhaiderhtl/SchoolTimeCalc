using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

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