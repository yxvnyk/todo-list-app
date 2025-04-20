namespace TodoListApp.WebApi.Helpers.Filters;

public class CommentFilter : PaginationFilter
{
    public int TaskId { get; set; }
}
