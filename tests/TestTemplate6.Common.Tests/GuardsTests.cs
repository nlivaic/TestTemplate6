using System;
using TestTemplate6.Common.Base;
using TestTemplate6.Common.Exceptions;
using Xunit;

namespace TestTemplate6.Common.Tests
{
    public class GuardsTests
    {
        [Fact]
        public void GuardsTests_NonNullOnNonNull_Passes()
        {
            // Arrange
            var entity = new TestEntity();

            // Act, Assert
            Guards.Guards.NonNull<TestEntity>(entity, default(Guid));
        }

        [Fact]
        public void GuardsTests_NonNullOnNull_Throws()
        {
            // Arrange, Act, Assert
            Assert.Throws<EntityNotFoundException>(() =>
                Guards.Guards.NonNull<TestEntity>(null, default(Guid)));
        }

        [Fact]
        public void GuardsTests_NonEmptyStringOnNonEmptyString_Passes()
        {
            // Arrange
            var str = "target string";

            // Act, Assert
            Guards.Guards.NonEmpty(str, "parameter");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GuardsTests_NonEmptyStringOnNullOrWhiteSpaceString_Throws(string str)
        {
            // Arrange, Act, Assert
            Assert.Throws<BusinessException>(() =>
                Guards.Guards.NonEmpty(str, "parameter"));
        }

        [Fact]
        public void GuardsTests_NonDefaultOnNonDefault_Passes()
        {
            // Arrange
            var target = new TestEntity();

            // Act, Assert
            Guards.Guards.NonDefault(target, string.Empty);
        }

        [Fact]
        public void GuardsTests_NonDefaultOnDefault_Throws()
        {
            // Arrange, Act, Assert
            Assert.Throws<BusinessException>(
                () => Guards.Guards.NonDefault(default(TestEntity), string.Empty));
        }

        [Fact]
        public void GuardsTests_NonDefaultGuidOnNonDefaultGuid_Passes()
        {
            // Arrange
            var target = Guid.NewGuid();

            // Act, Assert
            Guards.Guards.NonDefault(target, string.Empty);
        }

        [Fact]
        public void GuardsTests_NonDefaultGuidOnDefaultGuid_Throws()
        {
            // Arrange, Act, Assert
            Assert.Throws<BusinessException>(
                () => Guards.Guards.NonDefault(default(Guid), string.Empty));
        }

        [Fact]
        public void GuardsTests_NonDefaultDateTimeOnNonDefaultDateTime_Passes()
        {
            // Arrange
            var target = DateTime.UtcNow;

            // Act, Assert
            Guards.Guards.NonDefault(target, string.Empty);
        }

        [Fact]
        public void GuardsTests_NonDefaultDateTimeOnDefaultDateTime_Throws()
        {
            // Arrange, Act, Assert
            Assert.Throws<BusinessException>(
                () => Guards.Guards.NonDefault(default(DateTime), string.Empty));
        }

        [Fact]
        public void GuardsTests_ValidEmailOnValidEmail_Passes()
        {
            // Arrange
            var email = "a.b@c.com";

            // Act, Assert
            Guards.Guards.ValidEmail(email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("a")]
        [InlineData("a.")]
        [InlineData("a.b")]
        [InlineData("a.b@")]
        [InlineData("@")]
        [InlineData("a.b@c")]
        [InlineData("a.b@c.")]
        public void GuardsTests_ValidEmailOnInvalidOrNullOrWhiteSpaceEmail_Throws(string email)
        {
            // Arrange, Act, Assert
            Assert.Throws<BusinessException>(() =>
                Guards.Guards.ValidEmail(email));
        }

        private class TestEntity : BaseEntity<Guid>
        {
        }
    }
}
