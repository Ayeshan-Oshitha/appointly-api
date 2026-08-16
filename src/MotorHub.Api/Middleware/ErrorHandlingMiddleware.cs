using MotorHub.Domain.Infrastructure.Exceptions;
using System.Text.Json;

namespace MotorHub.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "Internal Server Error";
            // Unmapped exceptions get a fixed message: ex.Message can carry connection strings and
            // other internal detail that must not reach the client. The real exception is logged.
            string message = "An unexpected error occurred.";

            if (ex is AppException appException)
            {
                statusCode = appException.StatusCode;
                message = appException.Message;
                title = appException.Title;
            }
            else
            {
                _logger.LogError(ex, "Unhandled exception while processing {Method} {Path}",
                    context.Request.Method, context.Request.Path);
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                title = title,
                error = message,
                errorCode = statusCode
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
