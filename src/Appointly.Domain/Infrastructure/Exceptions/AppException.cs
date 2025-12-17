using System.Net;

namespace Appointly.Domain.Infrastructure.Exceptions
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }
        public string Title { get; }
        public AppException(string message, int statusCode, string title)
            : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }

    public class BadRequestException : AppException
    {
        public BadRequestException(string message)
            : base(message, (int)HttpStatusCode.BadRequest, "Bad Request")
        {
        }
    }

    public class NotFoundException : AppException
    {
        public NotFoundException(string message)
            : base(message, (int)HttpStatusCode.NotFound, "Not Found")
        {
        }
    }

    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message)
            : base(message, (int)HttpStatusCode.Unauthorized, "Unauthorized")
        {
        }
    }

    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message)
            : base(message, (int)HttpStatusCode.Forbidden, "Forbidden")
        {
        }
    }

    public class ConflictException : AppException
    {
        public ConflictException(string message)
            : base(message, (int)HttpStatusCode.Conflict, "Conflict")
        {
        }
    }


}
