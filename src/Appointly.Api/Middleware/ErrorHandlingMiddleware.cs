using Appointly.Domain.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace Appointly.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "Internal Server Error";
            string message = ex.Message;

            if (ex is AppException appException)
            {
                statusCode = appException.StatusCode;
                message = appException.Message;
                title = appException.Title;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                title = title,
                error = message,
                errorCode = statusCode
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response)
   );
        }
    }
}
