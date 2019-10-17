using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeEnumerable, int[]> Enumerable_EqualData =>
            new TheoryData<RangeEnumerable, int[]> 
            {
                { null, null },
                { new RangeEnumerable(0), new int[] { } },
                { new RangeEnumerable(1), new int[] { 0 } },
                { new RangeEnumerable(3), new int[] { 0, 1, 2 } },
                { new RangeNonGenericEnumerable(3, 0), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(Enumerable_EqualData))]
        public void Enumerable_BeEqualTo_With_Equal_Should_NotThrow(RangeEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeEnumerable, int[], string> Enumerable_NotEqualData =>
            new TheoryData<RangeEnumerable, int[], string> 
            {
                { new RangeEnumerable(0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
                { new RangeEnumerable(1), new int[] { }, "Expected '' but found '0' with more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
                { new RangeEnumerable(3), new int[] { 0, 5, 2 }, "Expected '0, 5, 2' but found '0, 1, 2' that differs at index 1 when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualData))]
        public void Enumerable_BeEqualTo_With_NotEqual_Should_Throw(RangeEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeEnumerable, int[]>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<RangeEnumerable, int[], string> Enumerable_NotEqualNullData =>
            new TheoryData<RangeEnumerable, int[], string>
            {
                { null, new int[] { }, "Expected '' to be equivalent to '<null>' but it's not." },
                { new RangeEnumerable(0), null, "Expected '<null>' to be equivalent to '' but it's not." },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualNullData))]
        public void Enumerable_BeEqualTo_With_NotEqual_Null_Should_Throw(RangeEnumerable actual, int[] expected, string message)
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