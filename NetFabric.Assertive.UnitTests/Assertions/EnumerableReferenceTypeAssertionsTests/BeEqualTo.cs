using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeEnumerable, int[], string> NotEqualNullData =>
            new TheoryData<RangeEnumerable, int[], string>
            {
                { null, new int[] { }, "Expected '' to be equivalent to '<null>' but it's not." },
                { new RangeEnumerable(0), null, "Expected '<null>' to be equivalent to '' but it's not." },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Null_Should_Throw(RangeEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeEnumerable, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}