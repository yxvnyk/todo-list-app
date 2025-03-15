using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ITodoListWebApiService, TodoListWebApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:TodoListsApi"]);
});
builder.Services.TryAddScoped<ITodoListWebApiService, TodoListWebApiService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "todos",
    pattern: "{controller=Todos}/{action=Get}/{id?}");

app.MapControllerRoute(
    name: "todos",
    pattern: "{controller=Todos}/{action=Post}/{id?}");

app.MapControllerRoute(
    name: "todos",
    pattern: "{controller=Todos}/{action=Delete}/{id?}");

app.MapControllerRoute(
    name: "todos",
    pattern: "{controller=Todos}/{action=Put}/{id?}");

app.Run();
