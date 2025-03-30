using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Filters;

public class PaginationFilter
{
    [Range(0, int.MaxValue)]
    public int PageSize { get; set; } = 20;

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
}
