using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestTemplate6.Api.Helpers
{
    public class ValidationProblemDetailsFactory
    {
        public static ValidationProblemDetails Create(ActionContext actionContext)
        {
            var problemDetails = new ValidationProblemDetails(actionContext.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Detail = "See the errors property for more details.",
                Instance = actionContext.HttpContext.Request.Path
            };
            problemDetails.Extensions.Add("traceId", Activity.Current?.TraceId.ToString() ?? actionContext.HttpContext.TraceIdentifier);
            return problemDetails;
        }

        public static ValidationProblemDetails Create(HttpContext httpContext, IDictionary<string, string[]> errorDictionary)
        {
            var problemDetails = new ValidationProblemDetails(errorDictionary)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Detail = "See the errors property for more details.",
                Instance = httpContext.Request.Path,
                Status = StatusCodes.Status422UnprocessableEntity
            };
            problemDetails.Extensions.Add("traceId", Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier);
            return problemDetails;
        }

        public static ProblemDetails CreateNotFoundProblemDetails(HttpContext httpContext, string message)
        {
            var problemDetails = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Resource not found.",
                Detail = message,
                Status = StatusCodes.Status404NotFound
            };
            problemDetails.Extensions.Add("traceId", Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier);
            return problemDetails;
        }

        public static ProblemDetails CreateInternalServerErrorProblemDetails(HttpContext context)
        {
            var problemDetails = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "A general error occurred.",
                Detail = "Some kind of error occurred in the API. Please use provided Id and get in touch with support.",
                Status = StatusCodes.Status500InternalServerError
            };
            problemDetails.Extensions.Add("traceId", Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier);
            return problemDetails;
        }
    }
}
