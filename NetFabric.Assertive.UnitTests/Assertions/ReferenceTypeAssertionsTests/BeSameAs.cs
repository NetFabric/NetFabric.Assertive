using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ReferenceTypeAssertionsTests
    {
        public static TheoryData<object> BeSameAs_SameData =>
            new TheoryData<object>
            {
                { null },
                { new object() },
            };

        [Theory]
        [MemberData(nameof(BeSameAs_SameData))]
        public void BeSameAs_With_Equal_Should_NotThrow(object value)
        {
            // Arrange

            // Act
            _ = value.Must().BeSameAs(value);

            // Assert
        }

        public static TheoryData<object, object, string> BeSameAs_NotSameData =>
            new TheoryData<object, object, string>
            {
                { new object(), new object(), $"Not the same instance.{Environment.NewLine}Expected: System.Object{Environment.NewLine}Actual: System.Object" },
            };

        [Theory]
        [MemberData(nameof(BeSameAs_NotSameData))]
        public void BeSameAs_With_NotSame_Should_Throw(object actual, object expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeSameAs(expected);

            // Assert
            var exception = Assert.Throws<ExpectedAssertionException<object, object>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);;
        }
    }
}
