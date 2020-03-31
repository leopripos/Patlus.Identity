namespace Patlus.Common.Presentation.Responses.Errors
{
    public class NotFoundErrorDto : IDto
    {
        public readonly string Message;

        public NotFoundErrorDto(string message)
        {
            this.Message = message;
        }
    }
}
