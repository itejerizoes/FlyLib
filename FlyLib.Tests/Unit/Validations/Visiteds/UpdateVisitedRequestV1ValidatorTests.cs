using FluentValidation.TestHelper;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Visited.Requests;
using FlyLib.API.DTOs.v1.Visiteds.Requests;
using System.Collections.Generic;
using Xunit;

namespace FlyLib.Tests.Unit.Validations.Visiteds
{
    public class UpdateVisitedRequestV1ValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_Id_Is_Zero()
        {
            var validator = new UpdateVisitedRequestV1Validator();
            var model = new UpdateVisitedRequestV1(0, "user1", 1, new List<UpdatePhotoRequestV1>());
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var validator = new UpdateVisitedRequestV1Validator();
            var model = new UpdateVisitedRequestV1(1, "", 1, new List<UpdatePhotoRequestV1>());
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_ProvinceId_Is_Zero()
        {
            var validator = new UpdateVisitedRequestV1Validator();
            var model = new UpdateVisitedRequestV1(1, "user1", 0, new List<UpdatePhotoRequestV1>());
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ProvinceId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var validator = new UpdateVisitedRequestV1Validator();
            var model = new UpdateVisitedRequestV1(1, "user1", 1, new List<UpdatePhotoRequestV1>());
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
