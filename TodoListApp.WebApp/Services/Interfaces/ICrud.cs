using System.Net;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ICrud<T, TUpdate>
{
    Task<T?> GetByIdAsync(int id);

    Task<IEnumerable<T>?> GetAllAsync(int id);

    Task<HttpStatusCode?> DeleteAsync(int id);

    Task<HttpStatusCode?> AddAsync(T model);

    Task<HttpStatusCode?> UpdateAsync(TUpdate model, int id);
}
