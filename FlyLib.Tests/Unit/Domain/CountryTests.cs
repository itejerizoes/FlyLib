using FlyLib.Domain.Entities;
using System;
using Xunit;

namespace FlyLib.Tests.Unit.Domain
{
    public class CountryTests
    {
        [Fact]
        public void Country_ShouldThrowException_WhenNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Country(""));
        }
    }
}
