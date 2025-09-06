using System.Net;
using System.Text.Json;

namespace API_TCC.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ErrorHandlingMiddleware(RequestDelegate next, 
                                     ILogger<ErrorHandlingMiddleware> logger,
                                     IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado na requisição: {Method} {Path}", 
                    context.Request.Method, context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var statusCode = exception switch
            {
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ArgumentException => HttpStatusCode.BadRequest,
                InvalidOperationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };
            
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                message = statusCode == HttpStatusCode.InternalServerError 
                    ? "Erro interno do servidor" 
                    : exception.Message,
                details = _environment.IsDevelopment() 
                    ? exception.ToString() 
                    : "Ocorreu um erro inesperado",
                timestamp = DateTime.UtcNow,
                path = context.Request.Path,
                method = context.Request.Method
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}

