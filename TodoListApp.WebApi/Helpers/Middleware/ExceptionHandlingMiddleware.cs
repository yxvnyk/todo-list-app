using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace TodoListApp.WebApi.Helpers.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

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
