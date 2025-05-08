using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Components
{
    /// <summary>
    /// A view component that retrieves and displays the user's todo lists in the sidebar.
    /// </summary>
    public class ListsSideBarViewComponent : ViewComponent
    {
        private readonly ITodoListWebApiService apiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListsSideBarViewComponent"/> class.
        /// </summary>
        /// <param name="service">The service used to fetch the todo list data.</param>
        public ListsSideBarViewComponent(ITodoListWebApiService service)
        {
            this.apiService = service;
        }

        /// <summary>
        /// Retrieves the user's todo lists and returns the view with the list of todo lists.
        /// </summary>
        /// <returns>A view component result with the list of todo lists.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Retrieves the currently authenticated user from the claims principal
            var user = this.User as ClaimsPrincipal;

            // Extracts the user's unique identifier (User ID) from the claims
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // If the user is not authenticated, returns an empty list
            if (user == null)
            {
                return this.View(new List<TodoListDTO>());
            }

            // Fetches the todo lists associated with the current user using the apiService
            var list = await this.apiService.GetAllAsync(userId!);

            // Returns the view with the retrieved todo lists
            return this.View(list);
        }
    }
}
