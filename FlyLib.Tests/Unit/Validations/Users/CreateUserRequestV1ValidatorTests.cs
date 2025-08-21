using FluentValidation.TestHelper;
using FlyLib.API.DTOs.v1.Users.Requests;
using Xunit;

namespace FlyLib.Tests.Unit.Validations.Users
{
    public class CreateUserRequestV1ValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_DisplayName_Is_Empty()
        {
            var validator = new CreateUserRequestV1Validator();
            var model = new CreateUserRequestV1("", "Test");
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DisplayName);
        }

        [Fact]
        public void Should_Have_Error_When_AuthProvider_Is_Empty()
        {
            var validator = new CreateUserRequestV1Validator();
            var model = new CreateUserRequestV1("usuario", "");
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AuthProvider);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var validator = new CreateUserRequestV1Validator();
            var model = new CreateUserRequestV1("usuario", "Test");
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
