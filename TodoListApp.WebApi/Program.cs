using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Repository;
using TodoListApp.WebApi.Data.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:TodoListDb"]));

builder.Services.AddScoped<ITodoListDatabaseService, TodoListDatabaseService>();
builder.Services.AddScoped<ITaskDatabaseService, TaskDatabaseService>();
builder.Services.AddScoped<ITagDatabaseService, TagDatabaseService>();
builder.Services.AddScoped<ICommentDatabaseService, CommentDatabaseService>();

 //builder.Services.AddDbContext<TodoListDbContext>(opt =>
 //opt.UseInMemoryDatabase("TodoList")); // for testing api

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
