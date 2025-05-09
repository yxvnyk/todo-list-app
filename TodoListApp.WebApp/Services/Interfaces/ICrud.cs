using System.Net;

namespace TodoListApp.WebApp.Services.Interfaces
{
    /// <summary>
    /// Defines basic CRUD operations for a resource type.
    /// </summary>
    /// <typeparam name="T">The type of the resource.</typeparam>
    /// <typeparam name="TUpdate">The type used to update the resource.</typeparam>
    public interface ICrud<T, TUpdate>
    {
        /// <summary>
        /// Retrieves a resource by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the resource.</param>
        /// <returns>A task that represents the asynchronous operation, with a result of the resource or <c>null</c> if not found.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all resources for a specific user.
        /// </summary>
        /// <param name="id">The identifier of the user whose resources are to be fetched.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of resources or <c>null</c> if no resources are found.</returns>
        Task<IEnumerable<T>?> GetAllAsync(string id);

        /// <summary>
        /// Deletes a resource by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the resource to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation, with a status code indicating the result of the delete operation.</returns>
        Task<HttpStatusCode?> DeleteAsync(int id);

        /// <summary>
        /// Adds a new resource.
        /// </summary>
        /// <param name="model">The resource to be added.</param>
        /// <returns>A task that represents the asynchronous operation, with a status code indicating the result of the add operation.</returns>
        Task<HttpStatusCode?> AddAsync(T model);

        /// <summary>
        /// Updates an existing resource.
        /// </summary>
        /// <param name="model">The updated resource.</param>
        /// <param name="id">The identifier of the resource to be updated.</param>
        /// <returns>A task that represents the asynchronous operation, with a status code indicating the result of the update operation.</returns>
        Task<HttpStatusCode?> UpdateAsync(TUpdate model, int id);
    }
}
