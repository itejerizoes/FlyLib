using FlyLib.Domain.Entities;
using System;
using Xunit;

namespace FlyLib.Tests.Unit.Domain
{
    public class ProvinceTests
    {
        [Fact]
        public void Province_ShouldThrowException_WhenNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Province(""));
        }
    }
}
