using System.Net;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ICrud<T, TUpdate>
{
    Task<IEnumerable<T>?> GetAllAsync(int id);

    Task<HttpStatusCode?> DeleteAsync(int id);

    Task<HttpStatusCode?> AddAsync(T model);

    Task<HttpStatusCode?> UpdateAsync(TUpdate model, int id);
}
