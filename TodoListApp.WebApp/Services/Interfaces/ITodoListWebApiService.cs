using System.Net;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoListWebApiService : ICrud<TodoListDTO, TodoListUpdateDTO, TodoListPaging>
{
}
