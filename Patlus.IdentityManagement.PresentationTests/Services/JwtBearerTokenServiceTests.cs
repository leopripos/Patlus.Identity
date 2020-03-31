using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Patlus.Common.Presentation.Security;
using Patlus.Common.UseCase.Security;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Presentation.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Patlus.IdentityManagement.PresentationTests.Services
{
    [Trait("UT-Class", "Services/JwtBearerTokenServiceTests")]
    public sealed class JwtBearerTokenServiceTests : IDisposable
    {
        private readonly Mock<IOptions<AccessTokenOptions>> _mockAccessTokenOptions;
        private readonly Mock<IOptions<RefreshTokenOptions>> _mockRefreshTokenOptions;
        private readonly Mock<IIdentifierService> _mockIdentifierService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<ITokenStorageService> _mockTokenStorageService;

        public JwtBearerTokenServiceTests()
        {
            _mockAccessTokenOptions = new Mock<IOptions<AccessTokenOptions>>();
            _mockAccessTokenOptions.Setup(e => e.Value).Returns(new AccessTokenOptions()
            {
                Issuer = "https://localhost/",
                Audience = "https://localhost/",
                Version = "1.0",
                Key = "12321maddraed213211212d",
                Duration = 60 // 1 hour
            });

            _mockRefreshTokenOptions = new Mock<IOptions<RefreshTokenOptions>>();
            _mockRefreshTokenOptions.Setup(e => e.Value).Returns(new RefreshTokenOptions()
            {
                Issuer = "https://localhost/",
                Audience = "https://localhost/",
                Version = "1.0",
                Key = "sadasdsad122e1csdgf32133",
                Duration = 3360 // 1 week
            });

            _mockIdentifierService = new Mock<IIdentifierService>();
            _mockIdentifierService.Setup(e => e.NewGuid()).Returns(new Guid("e599ecfa-0c7f-402f-a1ae-f3c936d0824b"));

            _mockTimeService = new Mock<ITimeService>();
            _mockTimeService.SetupGet(e => e.Now).Returns(new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero));
            _mockTimeService.SetupGet(e => e.NowDateTime).Returns(_mockTimeService.Object.Now.UtcDateTime);

            _mockTokenStorageService = new Mock<ITokenStorageService>();
        }

        public void Dispose()
        {
            _mockTokenStorageService.Reset();
        }


        [Fact()]
        public void Create_Return_Valid_Token()
        {
            // Arrange
            var identityId = new Guid("ce818a95-a5bc-4901-ad51-00b964efc7c0");
            var poolId = new Guid("2442cb04-5552-487c-ad31-ebc4ad990e84");
            var authKey = new Guid("63d21566-4c1f-410c-ac5a-8449bd55b75e");
            var expectedAcessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlNTk5ZWNmYS0wYzdmLTQwMmYtYTFhZS1mM2M5MzZkMDgyNGIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3ZlcnNpb24iOiIxLjAiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImNlODE4YTk1LWE1YmMtNDkwMS1hZDUxLTAwYjk2NGVmYzdjMCIsInBvb2wiOiIyNDQyY2IwNC01NTUyLTQ4N2MtYWQzMS1lYmM0YWQ5OTBlODQiLCJuYmYiOjE1Nzc4NDA0NjEsImV4cCI6MTU3Nzg0NDA2MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3QvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3QvIn0.XA_IRqqGOKWzgxaK4z9XKnKO_PCbw_WEMex-fIoCUyU";
            var expectedRefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlNTk5ZWNmYS0wYzdmLTQwMmYtYTFhZS1mM2M5MzZkMDgyNGIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3ZlcnNpb24iOiIxLjAiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImNlODE4YTk1LWE1YmMtNDkwMS1hZDUxLTAwYjk2NGVmYzdjMCIsInBvb2wiOiIyNDQyY2IwNC01NTUyLTQ4N2MtYWQzMS1lYmM0YWQ5OTBlODQiLCJuYmYiOjE1Nzc4NDA0NjEsImV4cCI6MTU3ODA0MjA2MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3QvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3QvIn0.9Y4mdKLBk3rnzKIthcPi9ywUj7bx4vbQv7Fopt6GLtQ";

            var claims = new List<Claim>()
            {
                new Claim(SecurityClaimTypes.Subject, identityId.ToString()),
                new Claim(SecurityClaimTypes.Pool, poolId.ToString())
            };

            var tokenService = new JwtBearerTokenService(
                    accessTokenOptions: _mockAccessTokenOptions.Object,
                    refreshTokenOptions: _mockRefreshTokenOptions.Object,
                    identifierService: _mockIdentifierService.Object,
                    timeService: _mockTimeService.Object,
                    tokenStorageService: _mockTokenStorageService.Object
                );

            // Act
            var token = tokenService.Create(authKey, claims);

            token.Should().NotBeNull();
            token.Scheme.Should().Be("Bearer");
            token.CreatedTime.Should().Be(_mockTimeService.Object.Now);
            token.Access.Should().Be(expectedAcessToken);
            token.Refresh.Should().Be(expectedRefreshToken);
        }

        [Fact()]
        public void TryParseRefreshToken_Should_Return_True()
        {
            // Arrange
            var identityId = new Guid("ce818a95-a5bc-4901-ad51-00b964efc7c0");
            var poolId = new Guid("2442cb04-5552-487c-ad31-ebc4ad990e84");
            var tokenId = _mockIdentifierService.Object.NewGuid();
            var versionId = "1.0";
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlNTk5ZWNmYS0wYzdmLTQwMmYtYTFhZS1mM2M5MzZkMDgyNGIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3ZlcnNpb24iOiIxLjAiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImNlODE4YTk1LWE1YmMtNDkwMS1hZDUxLTAwYjk2NGVmYzdjMCIsInBvb2wiOiIyNDQyY2IwNC01NTUyLTQ4N2MtYWQzMS1lYmM0YWQ5OTBlODQiLCJuYmYiOjE1Nzc4NDA0NjEsImV4cCI6MTU3ODA0MjA2MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3QvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3QvIn0.9Y4mdKLBk3rnzKIthcPi9ywUj7bx4vbQv7Fopt6GLtQ";

            var tokenService = new JwtBearerTokenService(
                    accessTokenOptions: _mockAccessTokenOptions.Object,
                    refreshTokenOptions: _mockRefreshTokenOptions.Object,
                    identifierService: _mockIdentifierService.Object,
                    timeService: _mockTimeService.Object,
                    tokenStorageService: _mockTokenStorageService.Object
                );

            // Act
            var result = tokenService.TryParseRefreshToken(refreshToken, out ClaimsPrincipal claimsPrincipal);

            // Assert
            result.Should().BeTrue();
            claimsPrincipal.FindFirst(SecurityClaimTypes.TokenId).Value.Should().Be(tokenId.ToString());
            claimsPrincipal.FindFirst(SecurityClaimTypes.Version).Value.Should().Be(versionId);
            claimsPrincipal.FindFirst(SecurityClaimTypes.Subject).Value.Should().Be(identityId.ToString());
            claimsPrincipal.FindFirst(SecurityClaimTypes.Pool).Value.Should().Be(poolId.ToString());
        }

        [Fact()]
        public void TryParseRefreshToken_Should_Return_False_If_Invalid_Token()
        {
            // Arrange
            var refreshToken = "invalidrefreshtoken";

            var tokenService = new JwtBearerTokenService(
                    accessTokenOptions: _mockAccessTokenOptions.Object,
                    refreshTokenOptions: _mockRefreshTokenOptions.Object,
                    identifierService: _mockIdentifierService.Object,
                    timeService: _mockTimeService.Object,
                    tokenStorageService: _mockTokenStorageService.Object
                );

            // Act
            var result = tokenService.TryParseRefreshToken(refreshToken, out _);

            // Assert
            result.Should().BeFalse();
        }

        [Fact()]
        public void ValidateRefreshToken_Should_Return_True()
        {
            // Arrage
            var authKey = new Guid("63d21566-4c1f-410c-ac5a-8449bd55b75e");
            var tokenId = _mockIdentifierService.Object.NewGuid();

            var claims = new List<Claim>()
            {
                new Claim(SecurityClaimTypes.TokenId, tokenId.ToString()),
            };

            var claimIdentity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(claimIdentity);

            _mockTokenStorageService.Setup(e => e.HasToken(tokenId, authKey)).Returns(true);

            var tokenService = new JwtBearerTokenService(
                    accessTokenOptions: _mockAccessTokenOptions.Object,
                    refreshTokenOptions: _mockRefreshTokenOptions.Object,
                    identifierService: _mockIdentifierService.Object,
                    timeService: _mockTimeService.Object,
                    tokenStorageService: _mockTokenStorageService.Object
                );

            // Act
            var result =  tokenService.ValidateRefreshToken(authKey, principal);

            // Assert
            result.Should().BeTrue();
        }

        [Fact()]
        public void ValidateRefreshToken_Should_Return_False_If_Not_In_Storage()
        {
            // Arrage
            var authKey = new Guid("63d21566-4c1f-410c-ac5a-8449bd55b75e");
            var tokenId = _mockIdentifierService.Object.NewGuid();

            var claims = new List<Claim>()
            {
                new Claim(SecurityClaimTypes.TokenId, tokenId.ToString()),
            };

            var claimIdentity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(claimIdentity);

            _mockTokenStorageService.Setup(e => e.HasToken(tokenId, authKey)).Returns(false);

            var tokenService = new JwtBearerTokenService(
                    accessTokenOptions: _mockAccessTokenOptions.Object,
                    refreshTokenOptions: _mockRefreshTokenOptions.Object,
                    identifierService: _mockIdentifierService.Object,
                    timeService: _mockTimeService.Object,
                    tokenStorageService: _mockTokenStorageService.Object
                );

            // Act
            var result = tokenService.ValidateRefreshToken(authKey, principal);

            // Assert
            result.Should().BeFalse();
        }

        [Fact()]
        public void ValidateRefreshToken_Should_Return_False_If_No_TokenId_Claim()
        {
            // Arrage
            var authKey = new Guid("63d21566-4c1f-410c-ac5a-8449bd55b75e");
            var tokenId = _mockIdentifierService.Object.NewGuid();

            var claims = new List<Claim>(){};
            var claimIdentity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(claimIdentity);

            _mockTokenStorageService.Setup(e => e.HasToken(tokenId, authKey)).Returns(true);

            var tokenService = new JwtBearerTokenService(
                    accessTokenOptions: _mockAccessTokenOptions.Object,
                    refreshTokenOptions: _mockRefreshTokenOptions.Object,
                    identifierService: _mockIdentifierService.Object,
                    timeService: _mockTimeService.Object,
                    tokenStorageService: _mockTokenStorageService.Object
                );

            // Act
            var result = tokenService.ValidateRefreshToken(authKey, principal);

            // Assert
            result.Should().BeFalse();
        }
    }
}