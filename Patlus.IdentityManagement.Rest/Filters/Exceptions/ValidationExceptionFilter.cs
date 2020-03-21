using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Patlus.IdentityManagement.Rest.Filters.Exceptions
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviourOptions;

        public ValidationExceptionFilter(IOptions<ApiBehaviorOptions> apiBehaviourOptions)
        {
            _apiBehaviourOptions = apiBehaviourOptions;
        }

        public void OnException(ExceptionContext context)
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
    }
}
