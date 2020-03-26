namespace Patlus.IdentityManagement.UseCase.Services
{
    public interface IPasswordService
    {
        string GeneratePasswordHash(string password);
        bool ValidatePasswordHash(string passwordHash, string password);
    }
}
