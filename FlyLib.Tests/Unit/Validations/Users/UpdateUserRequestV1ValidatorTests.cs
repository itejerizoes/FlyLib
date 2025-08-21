using FluentValidation.TestHelper;
using FlyLib.API.DTOs.v1.Users.Requests;
using Xunit;

namespace FlyLib.Tests.Unit.Validations.Users
{
    public class UpdateUserRequestV1ValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            var validator = new UpdateUserRequestV1Validator();
            var model = new UpdateUserRequestV1("", "usuario", "Test");
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_DisplayName_Is_Empty()
        {
            var validator = new UpdateUserRequestV1Validator();
            var model = new UpdateUserRequestV1("id", "", "Test");
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DisplayName);
        }

        [Fact]
        public void Should_Have_Error_When_AuthProvider_Is_Empty()
        {
            var validator = new UpdateUserRequestV1Validator();
            var model = new UpdateUserRequestV1("id", "usuario", "");
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AuthProvider);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var validator = new UpdateUserRequestV1Validator();
            var model = new UpdateUserRequestV1("id", "usuario", "Test");
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
