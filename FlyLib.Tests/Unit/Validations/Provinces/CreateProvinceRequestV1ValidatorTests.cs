using FluentValidation.TestHelper;
using FlyLib.API.DTOs.v1.Provinces.Requests;
using Xunit;

namespace FlyLib.Tests.Unit.Validations.Provinces
{
    public class CreateProvinceRequestV1ValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var validator = new CreateProvinceRequestV1Validator();
            var model = new CreateProvinceRequestV1("", 1);
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_CountryId_Is_Zero()
        {
            var validator = new CreateProvinceRequestV1Validator();
            var model = new CreateProvinceRequestV1("Buenos Aires", 0);
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CountryId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var validator = new CreateProvinceRequestV1Validator();
            var model = new CreateProvinceRequestV1("Buenos Aires", 1);
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
