using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Text;

namespace Patlus.IdentityManagement.Presentation.Services
{
    public class HMACSHA1PasswordService : IPasswordService
    {
        private readonly IOptions<PasswordOptions> _options;

        public HMACSHA1PasswordService(IOptions<PasswordOptions> options)
        {
            _options = options;
        }

        public string GeneratePasswordHash(string password)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Encoding.ASCII.GetBytes(_options.Value.Salt),
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
