using FlyLib.Domain.Entities;
using System;
using Xunit;

namespace FlyLib.Tests.Unit.Domain
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldThrowException_WhenUserNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new User(""));
        }
    }
}
