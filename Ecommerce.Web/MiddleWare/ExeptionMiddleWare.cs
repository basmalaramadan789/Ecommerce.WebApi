using Ecommerce.Web.Errors;
using System;
using System.Net;
using System.Text.Json;

namespace Ecommerce.Web.MiddleWare;

public class ExeptionMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExeptionMiddleWare> _logger;
    private readonly IHostEnvironment _env;

    public ExeptionMiddleWare(RequestDelegate next, ILogger<ExeptionMiddleWare> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ApiExeption((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                : new ApiExeption((int)HttpStatusCode.InternalServerError);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}

