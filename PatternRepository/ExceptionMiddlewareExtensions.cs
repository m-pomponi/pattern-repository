using ExceptionMiddleware.CustomException;

namespace ExceptionMiddleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app, Serilog.ILogger logger)
        {
            app.UseMiddleware<CustomExceptionMiddleware>(logger);
        }
    }
}
