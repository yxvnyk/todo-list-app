using TodoListApp.DataAccess.Filters.Enums;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.DataAccess.Filters;

public class TaskFilter : PaginationFilter
{
    public string? TextInTitle { get; set; }

    public Status? Status { get; set; }

    public string? TagName { get; set; }

    public string? AssigneeId { get; set; }

    public string? OwnerId { get; set; }

    public int TodoListId { get; set; }

    public Overdue? Overdue { get; set; }

    public string? SortBy { get; set; }

    public bool IsDescending { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CreationDate { get; set; }
}
