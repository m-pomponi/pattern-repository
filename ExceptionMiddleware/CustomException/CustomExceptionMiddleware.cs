using ExceptionMiddleware.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionMiddleware.CustomException
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //Add specific exception to handle
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentOutOfRangeException aoEx)
            {
                _logger.Error($"A new argument exception has been thrown: {aoEx}");
                await HandleExceptionAsync(httpContext, aoEx);
            }
            catch (NullReferenceException nrEx)
            {
                _logger.Error($"A new null reference exception has been thrown: {nrEx}");
                await HandleExceptionAsync(httpContext, nrEx);
            }
            catch (Exception ex)
            {
                _logger.Error($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //Add exception to customize the message
            var message = exception switch
            {
                NullReferenceException => "Null reference error from the custom middleware",
                ArgumentOutOfRangeException => "Argument not valid",
                _ => "Unexpected error. Try again later."
            };

            await context.Response.WriteAsync(new ErrorDetail()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
