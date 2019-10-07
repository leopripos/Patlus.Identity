using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Patlus.Identity.UseCase.Services;
using System;
using System.Text;

namespace Patlus.Identity.Rest.Services
{
    public class HMACSHA1PasswordService : IPasswordService
    {
        private readonly IConfiguration configuration;
        public HMACSHA1PasswordService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GeneratePasswordHash(string password)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Encoding.ASCII.GetBytes(configuration["Authentication:Password:Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
        }

        public bool ValidatePasswordHash(string passwordHash, string password)
        {
            return passwordHash.Equals(GeneratePasswordHash(password));
        }
    }
}
