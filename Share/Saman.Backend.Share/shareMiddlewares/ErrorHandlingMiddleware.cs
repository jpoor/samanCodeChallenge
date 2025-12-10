using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Saman.Backend.Share.shareExceptions;
using Saman.Backend.Share.shareExceptions.Rsx;

namespace Saman.Backend.Share.shareMiddlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exp)
            {
                // INIT ERROR MESSAGE
                string errorMessage = string.IsNullOrWhiteSpace(exp.Message)
                       ? Exception_Rsx.ResourceManager.GetString(exp.GetType().Name) ?? Exception_Rsx.Exception_UnhandledError
                       : (exp is baseException) 
                              ? exp.Message 
                              : Exception_Rsx.ResourceManager.GetString("Exception_InternalError") ?? Exception_Rsx.Exception_InternalServerError;

                // LOG ERROR
                _logger.LogWarning(exp, exp.HResult.ToString() + ": " + exp.Message);

                // ERROR CODE
                context.Response.StatusCode = exp.HResult > 0 
                                            ? exp.HResult 
                                            : StatusCodes.Status500InternalServerError;

                // ERROR MESSAGE
                context.Response.ContentType = "application/text; charset=utf-8";
                await context.Response.WriteAsync(errorMessage);
            }
        }

        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
    }
}
