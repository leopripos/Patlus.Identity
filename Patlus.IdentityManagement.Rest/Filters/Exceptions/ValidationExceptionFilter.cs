using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Patlus.IdentityManagement.Rest.Filters.Exceptions
{
    public class ValidationExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviourOptions;

        public int Order { get; set; } = int.MaxValue - 10;

        public ValidationExceptionFilter(IOptions<ApiBehaviorOptions> apiBehaviourOptions)
        {
            _apiBehaviourOptions = apiBehaviourOptions;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                foreach (var failure in exception.Errors)
                {
                    context.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                context.Result = _apiBehaviourOptions.Value.InvalidModelStateResponseFactory(context);

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        { }
    }
}
