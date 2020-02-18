using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Text;

namespace Patlus.IdentityManagement.Rest.Services
{
    public class HMACSHA1PasswordService : IPasswordService
    {
        private readonly IConfiguration passwordConfiguration;
        public HMACSHA1PasswordService(IConfigurationSection passwordConfiguration)
        {
            this.passwordConfiguration = passwordConfiguration;
        }
        public string GeneratePasswordHash(string password)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Encoding.ASCII.GetBytes(passwordConfiguration.GetValue<string>("Salt")),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
        }

        public bool ValidatePasswordHash(string passwordHash, string password)
        {
            return passwordHash.Equals(GeneratePasswordHash(password), StringComparison.Ordinal);
        }
    }
}
