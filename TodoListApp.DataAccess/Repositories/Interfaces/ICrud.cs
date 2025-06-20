namespace TodoListApp.DataAccess.Repositories.Interfaces;

public interface ICrud<TEntity, TFilter>
{
    IQueryable<TEntity> GetAllAsync(TFilter filter);

    Task CreateAsync(TEntity entity);

    Task SaveChangesAsync();

    Task<TEntity?> GetByIdAsync(int id);

    Task<bool> DeleteByIdAsync(int id);
}
