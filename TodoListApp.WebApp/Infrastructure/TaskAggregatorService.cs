using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Infrastructure;

public class TaskAggregatorService
{
    private readonly ITaskWebApiService taskWebApi;
    private readonly ITagWebApiService tagWebApi;
    private readonly ICommentWebApiService commentWebApi;

    public TaskAggregatorService(ITaskWebApiService taskWebApi, ITagWebApiService tagWebApi, ICommentWebApiService commentWebApi)
    {
        this.taskWebApi = taskWebApi;
        this.tagWebApi = tagWebApi;
        this.commentWebApi = commentWebApi;
    }

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
            OwnerId = ownerId,
        };
    }
}
