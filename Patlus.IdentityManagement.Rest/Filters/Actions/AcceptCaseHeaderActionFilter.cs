using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Patlus.IdentityManagement.Rest.Responses.Content;

namespace Patlus.IdentityManagement.Rest.Filters.Actions
{
    public class AcceptCaseHeaderActionFilter : IResourceFilter
    {
        public static readonly string AcceptCaseHeader = "Accept-Case";
        public static readonly string PascalCaseValue = "pascal";
        public static readonly string CamelCaseValue = "camel";

        public void OnResourceExecuted(ResourceExecutedContext context)
        { }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var mvcOptions = httpContext.RequestServices.GetRequiredService<IOptions<MvcOptions>>();
            var jsonOptions = httpContext.RequestServices.GetRequiredService<IOptionsSnapshot<JsonOptions>>();
            if (mvcOptions.Value.RespectBrowserAcceptHeader)
            {
                if (httpContext.Request.Headers.TryGetValue(AcceptCaseHeader, out StringValues values))
                {
                    var caseValue = values.ToString().ToLower();

                    if (caseValue == PascalCaseValue)
                    {
                        jsonOptions.Value.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicies.PascalCase;
                        jsonOptions.Value.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicies.PascalCase;
                    }
                    else if (caseValue == CamelCaseValue)
                    {
                        jsonOptions.Value.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicies.CamelCase;
                        jsonOptions.Value.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicies.CamelCase;
                    }
                    else
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                        context.Result = new ObjectResult(
                            new ValidationErrorDto(
                                $"Not acceptable {AcceptCaseHeader} with value `{caseValue}`. It should be `{PascalCaseValue} ` or `{CamelCaseValue}`"
                            )
                        );
                    }
                }
            }
        }
    }
}
