using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeAsyncEnumerable, int[], string> NotEqualNullData =>
            new TheoryData<RangeAsyncEnumerable, int[], string>
            {
                { null, new int[] { }, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { new RangeAsyncEnumerable(0), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Null_Should_Throw(RangeAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must()
                .BeAsyncEnumerableOf<int>()
                .BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeAsyncEnumerable, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}