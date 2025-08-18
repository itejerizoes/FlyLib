using FlyLib.Domain.Entities;
using System;
using Xunit;

namespace FlyLib.Tests.Unit.Domain
{
    public class VisitedTests
    {
        [Fact]
        public void Visited_ShouldThrowException_WhenProvinceIsNull()
        {
            Assert.Throws<ArgumentException>(() => new Visited(0));
        }
    }
}
