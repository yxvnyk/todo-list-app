using System.Net.Sockets;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoListApp.WebApp.Infrastructure;
using TodoListApp.WebApp.Services;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

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
        });
    });
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.Run();
