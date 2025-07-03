using Microsoft.AspNetCore.Http;
using Sentry;

namespace App.Helpers.MiddleWares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                
                // Re-throw to let the framework handle the response
                throw;
            }
        }
    }
}
