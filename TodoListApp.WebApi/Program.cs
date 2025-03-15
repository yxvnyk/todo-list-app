using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<TodoListDbContext>(options =>
//    options.UseSqlServer(builder.Configuration["ConnectionStrings:TodoListDb"]));

builder.Services.AddScoped<ITodoListDatabaseService, TodoListDatabaseService>();

 builder.Services.AddDbContext<TodoListDbContext>(opt =>
 opt.UseInMemoryDatabase("TodoList")); // for testing api
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
