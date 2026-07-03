namespace MicroGym.Server.Middleware
{
    // Catches any unhandled exception from a controller or service,
    // logs it, and returns a clean JSON error response instead of an HTML crash page.
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate             _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception — {Method} {Path} | TraceId: {TraceId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.TraceIdentifier
                );

                context.Response.StatusCode  = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "An unexpected error occurred. Please try again.",
                    traceId = context.TraceIdentifier
                });
            }
        }
    }
}
