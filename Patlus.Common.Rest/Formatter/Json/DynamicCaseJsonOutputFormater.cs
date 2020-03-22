using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Patlus.Common.Rest.Formatter.Json
{
    public class DynamicCaseJsonOutputFormater : TextOutputFormatter
    {
        public DynamicCaseJsonOutputFormater()
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/*+json"));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var jsonOptions = context.HttpContext.RequestServices.GetRequiredService<IOptionsSnapshot<JsonOptions>>();

            return new SystemTextJsonOutputFormatter(jsonOptions.Value.JsonSerializerOptions).WriteResponseBodyAsync(context, selectedEncoding);
        }
    }
}
