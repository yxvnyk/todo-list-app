namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ICrud<T>
{
    Task<IEnumerable<T>> GetAll();

    Task Create(T model);

    Task<bool> Update(T model, int id);

    Task<T?> GetById(int id);

    Task<bool> DeleteById(int id);
}
