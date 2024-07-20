using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Core;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, 
                                    ILogger<ExceptionMiddleware> logger, 
                                    IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }


        public async Task InvokeAsync(HttpContext ctx) {
            try {
                await _next(ctx);
            } catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new AppException(ctx.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new AppException(ctx.Response.StatusCode, "Server Error");

                var opts = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, opts);

                await ctx.Response.WriteAsync(json);
            }
        }
    }
}