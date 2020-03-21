using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.Rest.Responses.Content;

namespace Patlus.IdentityManagement.Rest.Filters.Exceptions
{
    public class NotFoundExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException exception)
            {
                context.Result = new NotFoundObjectResult(
                    new NotFoundResultDto(exception.Message)
                );

                context.ExceptionHandled = true;
            }
        }
    }
}
