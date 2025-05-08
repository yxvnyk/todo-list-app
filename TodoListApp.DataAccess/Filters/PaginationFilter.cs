using System.ComponentModel.DataAnnotations;

namespace TodoListApp.DataAccess.Filters;

/// <summary>
/// Represents the pagination settings used for filtering results in queries.
/// </summary>
public class PaginationFilter
{
    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <remarks>
    /// The value must be a non-negative integer and cannot exceed the maximum value of an integer.
    /// </remarks>
    [Range(0, int.MaxValue)]
    public int PageSize { get; set; } = 5;

    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary>
    /// <remarks>
    /// The value must be a positive integer.
    /// </remarks>
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
}
