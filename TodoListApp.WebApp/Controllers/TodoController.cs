using Microsoft.AspNetCore.Mvc;

namespace TodoListApp.WebApp.Controllers
{
    [Controller]
    public class TodoController : Controller
    {
        [HttpGet]
        public IActionResult GetAllTodos(int id)
        {

            return View(id);
        }
    }
}
