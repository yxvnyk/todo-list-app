using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Infrastructure
{
    /// <summary>
    /// Service for aggregating task data, including task information, comments, tags, and owner details.
    /// </summary>
    /// <remarks>
    /// This service uses multiple APIs to retrieve task data, comments, tags, and owner information.
    /// It aggregates all this data into a single task view model.
    /// </remarks>
    public class TaskAggregatorService
    {
        private readonly ITaskWebApiService taskWebApi;
        private readonly ITagWebApiService tagWebApi;
        private readonly ICommentWebApiService commentWebApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAggregatorService"/> class.
        /// </summary>
        /// <param name="taskWebApi">The API service for working with tasks.</param>
        /// <param name="tagWebApi">The API service for working with tags.</param>
        /// <param name="commentWebApi">The API service for working with comments.</param>
        public TaskAggregatorService(ITaskWebApiService taskWebApi, ITagWebApiService tagWebApi, ICommentWebApiService commentWebApi)
        {
            this.taskWebApi = taskWebApi;
            this.tagWebApi = tagWebApi;
            this.commentWebApi = commentWebApi;
        }

        /// <summary>
        /// Aggregates all data for a task based on its identifier.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <returns>
        /// A <see cref="TaskViewModel"/> object containing the aggregated task, comments, tags, and owner details,
        /// or <c>null</c> if the task is not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the API services are not initialized.
        /// </exception>
        public async Task<TaskViewModel?> AggregateTask(int id)
        {
            var task = await this.taskWebApi.GetByIdAsync(id);
            if (task == null)
            {
                return null;
            }

            var ownerId = await this.taskWebApi.GetTaskOwnerId(id);
            var comments = await this.commentWebApi.GetAllByTaskAsync(id);
            var tags = await this.tagWebApi.GetAllByTaskAsync(id);

            return new TaskViewModel()
            {
                Task = task,
                Comments = comments,
                Tags = tags,
                OwnerId = ownerId ?? string.Empty,
            };
        }
    }
}
