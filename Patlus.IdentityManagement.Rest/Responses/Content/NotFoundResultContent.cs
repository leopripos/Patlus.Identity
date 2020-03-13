namespace Patlus.IdentityManagement.Rest.Responses.Content
{
    public class NotFoundResultDto
    {
        public readonly string Message;

        public NotFoundResultDto(string message)
        {
            this.Message = message;
        }
    }
}
