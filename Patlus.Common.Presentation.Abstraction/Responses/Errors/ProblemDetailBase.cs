using System.Collections.Generic;

namespace Patlus.Common.Presentation.Responses.Errors
{
    public abstract class ProblemDetailBase : IDto
    {
        public string? Message { get; set; }
        public IDictionary<string, string[]>? Details { get; set; }

        protected ProblemDetailBase(string message, IDictionary<string, string[]> details)
        {
            this.Message = message;
            this.Details = details;
        }
    }
}
