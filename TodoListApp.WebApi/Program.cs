using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Repository;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Helpers.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:TodoListDb"]));

builder.Services.AddScoped<ITodoListDatabaseService, TodoListDatabaseService>();
builder.Services.AddScoped<ITaskDatabaseService, TaskDatabaseService>();
builder.Services.AddScoped<ITagDatabaseService, TagDatabaseService>();
builder.Services.AddScoped<ICommentDatabaseService, CommentDatabaseService>();

builder.Services.AddScoped<ExceptionHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
