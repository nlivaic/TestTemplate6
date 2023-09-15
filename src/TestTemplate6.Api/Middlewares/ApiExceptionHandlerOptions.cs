using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestTemplate6.Api.Middlewares
{
    public class ApiExceptionHandlerOptions
    {
        public Action<HttpContext, Exception, ProblemDetails> ApiErrorHandler { get; set; }
        public Func<HttpContext, Exception, LogLevel> LogLevelHandler { get; set; }
    }
}
