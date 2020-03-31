using System.Collections.Generic;

namespace Patlus.Common.Presentation.Responses.Errors
{
    public class ValidationErrorDto : ProblemDetailBase
    {
        public ValidationErrorDto() : base(null!, null!) { }

        public ValidationErrorDto(string message, IDictionary<string, string[]> details)
            : base(message, details)
        {}

        public ValidationErrorDto(IDictionary<string, string[]> details)
            : this("One or more validation errors occurred", details)
        { }

        public ValidationErrorDto(string message) : this(message, new Dictionary<string, string[]>())
        { }
    }
}
