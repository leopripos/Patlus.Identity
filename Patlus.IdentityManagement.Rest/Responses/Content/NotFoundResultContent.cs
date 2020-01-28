namespace Patlus.IdentityManagement.Rest.Responses.Content
{
    public class NotFoundResultContent
    {
        public readonly string Message;

        public NotFoundResultContent(string message)
        {
            this.Message = message;
        }
    }
}
