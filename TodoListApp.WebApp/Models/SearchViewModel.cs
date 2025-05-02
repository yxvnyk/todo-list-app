using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Models;

public class SearchViewModel
{
    public TaskPaging? TaskPagging { get; set; }

    public string? Query { get; set; }

    public string? SearchBy { get; set; }
}
