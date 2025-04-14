namespace TodoListApp.WebApi.Models;

public class ResponseModel<T>
{
    public bool Success { get; set; }

    public string Message { get; set; }

    public T Data { get; set; }

    public IEnumerable<string>? Errors { get; set; }

    public ResponseModel()
    {
        this.Success = true;
    }
}
