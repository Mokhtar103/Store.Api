using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Shared.ErrorModels;
using System.Net;

namespace Store.Api.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandleNotFoundEndPoint(httpContext);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something Went Wrong {ex}");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleNotFoundEndPoint(HttpContext httpContext)
        {
            //httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = $"The End Point {httpContext.Request.Path} Not Found",
                StatusCode = (int)HttpStatusCode.NotFound,
            };


            await httpContext.Response.WriteAsync(response.ToString());
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message,
                StatusCode = httpContext.Response.StatusCode,
            };

            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException validationException => HandleValidationException(validationException, response),
                _ => (int)HttpStatusCode.InternalServerError,
            };

            response.StatusCode = httpContext.Response.StatusCode;

            await httpContext.Response.WriteAsync(response.ToString());
        }

        private int HandleValidationException(ValidationException ex, ErrorDetails errorDetails)
        {
            errorDetails.Errors = ex.Errors;
            return (int)HttpStatusCode.BadRequest;
        }
    }
}
