using System;
using Microsoft.AspNetCore.Builder;

namespace TestTemplate6.Api.Middlewares
{
    public static class ApiExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseApiExceptionHandler(
            this IApplicationBuilder builder) => builder.UseMiddleware<ApiExceptionHandlerMiddleware>();

        public static IApplicationBuilder UseApiExceptionHandler(
            this IApplicationBuilder builder,
            Action<ApiExceptionHandlerOptions> configureOptions)
        {
            var options = new ApiExceptionHandlerOptions();
            configureOptions(options);
            return builder.UseMiddleware<ApiExceptionHandlerMiddleware>(options);
        }
    }
}