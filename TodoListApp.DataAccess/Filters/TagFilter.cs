namespace TodoListApp.DataAccess.Filters;

public class TagFilter : PaginationFilter
{
    public int TaskId { get; set; }

    public int TodoListId { get; set; }
}
