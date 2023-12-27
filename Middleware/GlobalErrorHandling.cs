using Microsoft.AspNetCore.Authentication;

public class GlobalErrorHandling : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch
        {
            context.Response.StatusCode = 500;
            context.Response.Headers.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new { StatusCode = StatusCodes.Status500InternalServerError, Error = "Internal Server Error" });
        }
    }
}

public static class GlobalErrorHandlingExtesion
{

    public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalErrorHandling>();
    }

    public static IServiceCollection AddGlobalErrorHandling(this IServiceCollection collection)
    {
        return collection.AddSingleton<GlobalErrorHandling>();
    }
}