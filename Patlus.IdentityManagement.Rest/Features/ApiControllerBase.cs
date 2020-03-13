using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Patlus.IdentityManagement.Rest.Features
{
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <inheritdoc/>
        [NonAction]
        public override ActionResult ValidationProblem()
        {
            return ValidationProblem(ModelState);
        }

        /// <inheritdoc/>
        [NonAction]
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
