using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Patlus.IdentityManagement.Rest.Responses.Content;
using System.Collections.Generic;
using System.Text.Json;

namespace Patlus.IdentityManagement.Rest.Filters.Exceptions
{
    public class ValidationExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException exception && context.Controller is ControllerBase)
            {
                var details = new Dictionary<string, List<string>>();

                foreach (var validationsfailures in exception.Errors)
                {
                    var fieldName = JsonNamingPolicy.CamelCase.ConvertName(validationsfailures.PropertyName);

                    if (!details.ContainsKey(fieldName))
                    {
                        details.Add(fieldName, new List<string>(1));
                    }

                    details[fieldName].Add(validationsfailures.ErrorMessage);
                }

                context.Result = new BadRequestObjectResult(
                    new ValidationErrorResultContent("Validation error", details)
                );

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        { }
    }
}
