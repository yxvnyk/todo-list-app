using System;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.UserDataAccess;
using TodoListApp.UserDataAccess.Context;
using TodoListApp.WebApp.Infrastructure;
using TodoListApp.WebApp.Services;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
{
    _ = options.UseSqlServer(builder.Configuration["ConnectionStrings:UserDb"]);
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<UserDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Auth/login";
        options.LoginPath = "/Auth/login";
    });

builder.Services.AddHttpClient<ITodoListWebApiService, TodoListWebApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:TodoListsApi"]);
});

builder.Services.AddHttpClient<ITaskWebApiService, TaskWebApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:TodoListsApi"]);
});
builder.Services.AddHttpClient<ITagWebApiService, TagWebApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:TodoListsApi"]);
});

builder.Services.AddHttpClient<ICommentWebApiService, CommentWebApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:TodoListsApi"]);
});
builder.Services.TryAddScoped<ITodoListWebApiService, TodoListWebApiService>();
builder.Services.TryAddScoped<ITaskWebApiService, TaskWebApiService>();
builder.Services.TryAddScoped<ITagWebApiService, TagWebApiService>();
builder.Services.TryAddScoped<ICommentWebApiService, CommentWebApiService>();
builder.Services.TryAddScoped<TaskAggregatorService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = Text.Plain;

            await context.Response.WriteAsync("An exception was thrown.");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is SocketException)
            {
                await context.Response.WriteAsync("\nNo connection could be made because the target machine actively refused it.");
            }

            if (exceptionHandlerPathFeature?.Error is HttpRequestException)
            {
                await context.Response.WriteAsync("\nNo connection could be made because the target machine actively refused it.");
            }

            if (exceptionHandlerPathFeature?.Error is UnauthorizedAccessException)
            {
                context.Response.Redirect("/Auth/login");
                return;
            }
            else
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An error occurred.");
            }
        });
    });
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
