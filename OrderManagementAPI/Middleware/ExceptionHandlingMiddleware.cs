using System.Net;
using System.Text.Json;

namespace OrderManagementAPI.Middleware {
    // Middleware responsável por tratar exceções não capturadas na aplicação
    public class ExceptionHandlingMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        // Recebe o próximo middleware e o serviço de log
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        // Intercepta todas as requisições HTTP
        public async Task InvokeAsync(HttpContext context) {
            try {
                // Envia a requisição para o próximo middleware da pipeline
                await _next(context);
            } catch (Exception ex) {
                // Registra a exceção no log
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Define a resposta como JSON
                context.Response.ContentType = "application/json";

                // Retorna o código HTTP 500 (Internal Server Error)
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Objeto que será enviado na resposta
                var response = new {
                    status = 500,
                    title = "An unexpected error occurred.",
                    detail = "Please try again later or contact support if the problem persists."
                };

                // Converte o objeto para JSON e envia ao cliente
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}