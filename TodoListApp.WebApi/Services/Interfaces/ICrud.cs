namespace TodoListApp.WebApi.Data.Repository.Interfaces
{
    /// <summary>
    /// Defines basic CRUD (Create, Read, Update, Delete) operations for an entity type.
    /// </summary>
    /// <typeparam name="T">The type of the entity (DTO) to be managed by this repository.</typeparam>
    /// <typeparam name="TUpdate">The type of the model used for updating an entity.</typeparam>
    /// <typeparam name="TFilter">The type of the filter used to query or search for entities.</typeparam>
    public interface ICrud<T, TUpdate, TFilter>
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="model">The model representing the entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateAsync(T model);

        /// <summary>
        /// Updates an existing entity by its identifier.
        /// </summary>
        /// <param name="model">The updated model for the entity.</param>
        /// <param name="id">The identifier of the entity to update.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the update was successful.</returns>
        Task<bool> UpdateAsync(TUpdate model, int id);

        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the entity, or null if not found.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
        Task<bool> DeleteByIdAsync(int id);
    }
}
