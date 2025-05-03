namespace TodoListApp.WebApi.Models.DTO.PagingDTO;
public class CommentPaging
{
    public IEnumerable<TaskDTO>? Items { get; set; }

    public int? CurrentPage { get; set; }

    public int TotalCount { get; set; }

}
