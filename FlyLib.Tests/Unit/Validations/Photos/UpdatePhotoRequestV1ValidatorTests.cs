using FluentValidation.TestHelper;
using FlyLib.API.DTOs.v1.Photos.Requests;
using Xunit;

namespace FlyLib.Tests.Unit.Validations.Photos
{
    public class UpdatePhotoRequestV1ValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_Id_Is_Zero()
        {
            var validator = new UpdatePhotoRequestV1Validator();
            var model = new UpdatePhotoRequestV1(0, "http://valid.com", "desc", 1);
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PhotoId);
        }

        [Fact]
        public void Should_Have_Error_When_Url_Is_Empty()
        {
            var validator = new UpdatePhotoRequestV1Validator();
            var model = new UpdatePhotoRequestV1(1, "", "desc", 1);
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Url);
        }

        [Fact]
        public void Should_Have_Error_When_Url_Is_Invalid()
        {
            var validator = new UpdatePhotoRequestV1Validator();
            var model = new UpdatePhotoRequestV1(1, "not-a-url", "desc", 1);
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Url);
        }

        [Fact]
        public void Should_Have_Error_When_VisitedId_Is_Zero()
        {
            var validator = new UpdatePhotoRequestV1Validator();
            var model = new UpdatePhotoRequestV1(1, "http://valid.com", "desc", 0);
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.VisitedId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var validator = new UpdatePhotoRequestV1Validator();
            var model = new UpdatePhotoRequestV1(1, "http://valid.com", "desc", 1);
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
