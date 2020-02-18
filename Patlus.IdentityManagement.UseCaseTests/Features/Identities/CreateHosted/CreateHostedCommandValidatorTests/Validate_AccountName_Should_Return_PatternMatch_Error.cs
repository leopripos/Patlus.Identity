using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Services;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.CreateHosted.CreateHostedCommandValidatorTests
{
    public class Validate_AccountName_Should_Return_PatternMatch_Error
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_AccountName_Should_Return_PatternMatch_Error()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, CreateHostedCommand query)
        {
            // Arrange
            var validator = new CreateHostedCommandValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.PatternMatch);
        }

        class TestData : TheoryData<string, CreateHostedCommand>
        {
            public TestData()
            {
                // Start With
                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "1name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "2name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "3name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "4name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "5name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "6name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "7name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "8name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "9name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "~name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "!name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "@name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "#name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "$name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "%name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "^name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "&name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "*name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "(name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = ")name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "-name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "_name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "+name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "=name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = ":name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = ";name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "\"name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "'name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "`name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "{name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "}name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "[name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "]name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "\\name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "/name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "|name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "<name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = ">name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = ",name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = ".name",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "?name",
                    }
                );

                // Contain

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name~",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name!",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name@",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name#",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name$",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name%",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name^",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name&",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name*",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name(",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name)",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name-",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name_",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name+",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name=",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name:",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name;",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name\"",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name'",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name`",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name{",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name}",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name[",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name]",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name\\",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name/",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name|",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name<",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name>",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name,",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name.",
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        AccountName = "name?",
                    }
                );
            }
        }
    }
}
