using System.ComponentModel.DataAnnotations;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.WebApi.Filters;

public class TaskFilter
{
    public string? TextInTitle { get; set; }

    public Status? Status { get; set; }

    public string? TagName { get; set; }

    public string? AssigneeId { get; set; }

    public int TodoListId { get; set; }
}
