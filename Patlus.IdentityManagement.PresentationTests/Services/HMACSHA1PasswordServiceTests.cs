using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Patlus.IdentityManagement.Presentation.Services;
using Xunit;

namespace Patlus.IdentityManagement.PresentationTests.Services
{
    [Trait("UT-Class", "Services/HMACSHA1PasswordServiceTests")]
    public class HMACSHA1PasswordServiceTests
    {
        private readonly Mock<IOptions<PasswordOptions>> _mockOptions;
        public HMACSHA1PasswordServiceTests()
        {
            _mockOptions = new Mock<IOptions<PasswordOptions>>();
            _mockOptions.Setup(e => e.Value).Returns(new PasswordOptions() { 
                Salt = "passwordsaltfortesting"
            });
        }

        [Fact()]
        public void GeneratePasswordHash_Should_Return_Right_Hash()
        {
            // Arrange
            var passwordService = new HMACSHA1PasswordService(_mockOptions.Object);

            // Act
            var hash = passwordService.GeneratePasswordHash("testingpassword");

            // Assert
            hash.Should().Be("o0VcZ5f4zsKghPWeDaePft+4TGfCitW5BmcWEpEdcjU=");
        }

        [Fact()]
        public void ValidatePasswordHash_Should_Return_True()
        {
            // Arrange
            var passwordService = new HMACSHA1PasswordService(_mockOptions.Object);

            // Act
            var result = passwordService.ValidatePasswordHash("o0VcZ5f4zsKghPWeDaePft+4TGfCitW5BmcWEpEdcjU=", "testingpassword");

            // Assert
            result.Should().BeTrue();
        }

        [Fact()]
        public void ValidatePasswordHash_Should_Return_False()
        {
            // Arange
            var passwordService = new HMACSHA1PasswordService(_mockOptions.Object);

            // Act
            var result = passwordService.ValidatePasswordHash("o0VcZ5f4zsKghPWeDaePft+4TGfCitW5BmcWEpEdcjU=", "wrongpassword");

            // Assert
            result.Should().BeFalse();
        }
    }
}