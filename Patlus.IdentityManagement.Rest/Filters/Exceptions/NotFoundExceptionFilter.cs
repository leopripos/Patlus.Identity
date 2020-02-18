using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.Rest.Responses.Content;

namespace Patlus.IdentityManagement.Rest.Filters.Exceptions
{
    public class NotFoundExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 9;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is NotFoundException exception)
            {
                context.Result = new NotFoundObjectResult(
                    new NotFoundResultContent(exception.Message)
                );

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        { }
    }
}
