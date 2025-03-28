using TodoListApp.WebApi.Filters;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ICrud<T, TFilter>
{
    Task<IEnumerable<T>> GetAllAsync(TFilter filter);

    Task CreateAsync(T model);

    Task<bool> UpdateAsync(T model, int id);

    Task<T?> GetByIdAsync(int id);

    Task<bool> DeleteByIdAsync(int id);
}
