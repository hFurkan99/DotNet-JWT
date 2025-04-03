using App.Application;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace App.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            if (exception is HttpRequestException httpRequestException && httpRequestException.StatusCode.HasValue)
            {
                statusCode = httpRequestException.StatusCode.Value;
            }

            var errorAsDto = ServiceResult.Fail(exception.Message, statusCode);

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(errorAsDto, cancellationToken);

            return true;
        }
    }
}
