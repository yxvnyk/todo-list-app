using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Components
{
    /// <summary>
    /// A view component that retrieves and displays the user's to-do lists in the sidebar.
    /// </summary>
    public class ListsSideBarViewComponent : ViewComponent
    {
        private readonly ITodoListWebApiService apiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListsSideBarViewComponent"/> class.
        /// </summary>
        /// <param name="service">The service used to fetch the to-do list data.</param>
        public ListsSideBarViewComponent(ITodoListWebApiService service)
        {
            this.apiService = service;
        }

        /// <summary>
        /// Retrieves the user's to-do lists and returns the view with the list of to-do lists.
        /// </summary>
        /// <returns>A view component result with the list of to-do lists.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = this.User as ClaimsPrincipal;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (user == null)
            {
                return this.View(new List<TodoListDto>());
            }

            var list = await this.apiService.GetAllAsync(userId!);
            return this.View(list);
        }
    }
}
