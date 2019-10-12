using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Patlus.IdentityManagement.Rest.Filters.Exceptions
{
    public class ValidationExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException exception && context.Controller is ControllerBase controller)
            {

                foreach (var validationsfailures in exception.Errors)
                {
                    controller.ModelState.AddModelError(validationsfailures.PropertyName, validationsfailures.ErrorMessage);
                }

                context.Result = new BadRequestObjectResult(controller.ModelState);

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        { }
    }
}
