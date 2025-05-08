using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace TodoListApp.WebApi.Helpers.Middleware;

/// <summary>
/// Middleware that handles unhandled exceptions in the HTTP request pipeline
/// and returns appropriate HTTP error responses.
/// </summary>
public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logger used for logging exceptions.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is null.</exception>
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Invokes the middleware and handles exceptions thrown by downstream components.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="next">The next middleware in the request pipeline.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            await next(context);
        }
        catch (SqlException)
        {
            LoggerExtensions.LogError(this.logger, "SqlException");

            var details = new ProblemDetails()
            {
                Detail = "Database error has been occured",
                Title = "Database error",
                Status = 500,
                Type = "Database error",
            };

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(details);
        }
        catch (TimeoutException)
        {
            LoggerExtensions.LogError(this.logger, "TimeoutException");

            var details = new ProblemDetails()
            {
                Detail = "The request time out.",
                Title = "Timeout error",
                Status = 504,
                Type = "Timeout error",
            };

            context.Response.StatusCode = 504;
            await context.Response.WriteAsJsonAsync(details);
        }
        catch (Exception)
        {
            LoggerExtensions.LogError(this.logger, "An error has occured");

            context.Response.StatusCode = 500;
            throw;
        }
    }
}
