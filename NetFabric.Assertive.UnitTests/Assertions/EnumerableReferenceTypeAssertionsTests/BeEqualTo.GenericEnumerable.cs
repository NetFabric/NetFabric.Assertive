using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeGenericEnumerable, int[]> GenericEnumerable_EqualData =>
            new TheoryData<RangeGenericEnumerable, int[]> 
            {
                { null, null },
                { new RangeGenericEnumerable(0, 0, 0), new int[] { } },
                { new RangeGenericEnumerable(1, 1, 1), new int[] { 0 } },
                { new RangeGenericEnumerable(3, 3, 3), new int[] { 0, 1, 2 } },
                { new RangeReadOnlyCollection(3, 3, 3, 0), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(GenericEnumerable_EqualData))]
        public void GenericEnumerable_BeEqualTo_With_Equal_Should_NotThrow(RangeGenericEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeGenericEnumerable, int[], string> GenericEnumerable_NotEqualData =>
            new TheoryData<RangeGenericEnumerable, int[], string> 
            {
                { new RangeGenericEnumerable(0, 0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
                { new RangeGenericEnumerable(1, 0, 0), new int[] { }, "Expected '' but found '0' with more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },

                { new RangeGenericEnumerable(1, 0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'System.Collections.IEnumerable.GetEnumerator()'." },
                { new RangeGenericEnumerable(0, 1, 0), new int[] { }, "Expected '' but found '0' with more items when using 'System.Collections.IEnumerable.GetEnumerator()'." },

                { new RangeGenericEnumerable(1, 1, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'." },
                { new RangeGenericEnumerable(0, 0, 1), new int[] { }, "Expected '' but found '0' with more items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'." },
            };

        [Theory]
        [MemberData(nameof(GenericEnumerable_NotEqualData))]
        public void GenericEnumerable_BeEqualTo_With_NotEqual_Should_Throw(RangeGenericEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeGenericEnumerable, IEnumerable<int>>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}