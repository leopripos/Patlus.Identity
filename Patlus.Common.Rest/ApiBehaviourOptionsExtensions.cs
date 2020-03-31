using Microsoft.AspNetCore.Mvc;
using Patlus.Common.Presentation.Responses.Errors;
using System.Linq;

namespace Patlus.Common.Rest
{
    public static class ApiBehaviourOptionsExtensions
    {
        public static ApiBehaviorOptions AddDefaultOptions(this ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.ToDictionary(
                    item => item.Key,
                    item => item.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

                return new BadRequestObjectResult(
                    new ValidationErrorDto(errors)
                );
            };

            return options;
        }
    }
}
