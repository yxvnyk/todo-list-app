using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Helpers.Filters;

public class PaginationFilter
{
    [Range(0, int.MaxValue)]
    public int PageSize { get; set; } = 5;

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
}
