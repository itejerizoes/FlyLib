using FluentValidation.TestHelper;
using FlyLib.API.DTOs.v1.Countries.Requests;
using Xunit;

namespace FlyLib.Tests.Unit.Validations.Countries
{
    public class CreateCountryRequestV1ValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var validator = new CreateCountryRequestV1Validator();
            var model = new CreateCountryRequestV1("", "ARG");
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_IsoCode_Is_Empty()
        {
            var validator = new CreateCountryRequestV1Validator();
            var model = new CreateCountryRequestV1("Argentina", "");
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.IsoCode);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var validator = new CreateCountryRequestV1Validator();
            var model = new CreateCountryRequestV1("Argentina", "ARG");
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
