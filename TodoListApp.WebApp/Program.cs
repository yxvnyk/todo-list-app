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

if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}
else
{
    _ = app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
