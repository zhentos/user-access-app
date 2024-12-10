using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using System.Security.Claims;

namespace UserAccessApp.WebApi.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            var identity = httpContext.User.Identity as ClaimsIdentity;

            if (identity != null && identity.IsAuthenticated)
            {
                var userEmail = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty;
                if (userEmail != null)
                {
                    logger.LogError(exception,
                    $"Could not process a request on machine {Environment.MachineName} for the User {userEmail}.  TraceId: {traceId}");
                }
                else
                {
                    logger.LogError(exception,
                    $"Could not process a request on machine {Environment.MachineName}.  TraceId: {traceId} User emal = empty string.");
                }
            }
            else
            {
                logger.LogError(exception, exception.Message);
            }
            return true;
        }
    }
}
