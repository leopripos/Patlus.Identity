using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Patlus.Common.Rest.Filters.Actions;
using Patlus.Common.Rest.Filters.Exceptions;
using Patlus.Common.Rest.Formatters.Json;

namespace Patlus.Common.Rest
{
    public static class MvcOptionsExtensions
    {
        public static MvcOptions AddDynamicJsonCaseFormatter(this MvcOptions options)
        {
            options.RespectBrowserAcceptHeader = true;

            options.Filters.Add<AcceptCaseHeaderActionFilter>();

            options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
            options.OutputFormatters.Add(new DynamicCaseJsonOutputFormater());

            return options;
        }

        public static MvcOptions AddDefaultExceptionFilters(this MvcOptions options)
        {
            options.Filters.Add<NotFoundExceptionFilter>();
            options.Filters.Add<ValidationExceptionFilter>();

            return options;
        }
    }
}
