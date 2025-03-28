namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ICrud<T>
{
    Task<IEnumerable<T>> GetAllAsync();

    Task CreateAsync(T model);

    Task<bool> UpdateAsync(T model, int id);

    Task<T?> GetByIdAsync(int id);

    Task<bool> DeleteByIdAsync(int id);
}
