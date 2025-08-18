using FlyLib.Domain.Entities;
using System;
using Xunit;

namespace FlyLib.Tests.Unit.Domain
{
    public class PhotoTests
    {
        [Fact]
        public void Photo_ShouldThrowException_WhenUrlIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Photo(""));
        }
    }
}
