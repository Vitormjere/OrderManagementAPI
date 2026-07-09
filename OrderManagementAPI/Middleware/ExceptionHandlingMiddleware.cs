using System.Net;
using System.Text.Json;

namespace OrderManagementAPI.Middleware {
    public class ExceptionHandlingMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                // Deixa a requisição seguir normalmente pro resto do pipeline
                await _next(context);
            } catch (Exception ex) {
                // Se qualquer coisa não tratada estourar em algum lugar, cai aqui
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new {
                    status = 500,
                    title = "An unexpected error occurred.",
                    detail = "Please try again later or contact support if the problem persists."
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}