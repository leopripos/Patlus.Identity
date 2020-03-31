using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Patlus.Common.Presentation.Responses.Errors;
using Patlus.Common.UseCase.Exceptions;

namespace Patlus.Common.Rest.Filters.Exceptions
{
    public class NotFoundExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException exception)
            {
                context.Result = new NotFoundObjectResult(
                    new NotFoundErrorDto(exception.Message)
                );
            }
        }
    }
}
