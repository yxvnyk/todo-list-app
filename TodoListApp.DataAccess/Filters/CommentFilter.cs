namespace TodoListApp.DataAccess.Filters;

public class CommentFilter : PaginationFilter
{
    public int TaskId { get; set; }
}
