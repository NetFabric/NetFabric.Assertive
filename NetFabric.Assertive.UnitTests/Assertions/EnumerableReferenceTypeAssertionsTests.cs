using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<List<int>> EqualData =>
            new TheoryData<List<int>> 
            {
                { null },
                { new List<int>(new int[] { }) },
                { new List<int>(new int[] { 1 }) },
                { new List<int>(new int[] { 1, 2, 3 }) },
            };

        [Theory]
        [MemberData(nameof(EqualData))]
        public void BeEqualTo_With_Equal_Should_NotThrow(List<int> value)
        {
            // Arrange

            // Act
            value.Must().BeEnumerable<int>().BeEqualTo(value);

            // Assert
        }

        public static TheoryData<List<int>, List<int>, string> NotEqualData =>
            new TheoryData<List<int>, List<int>, string> 
            {
                { new List<int>(new int[] { }), new List<int>(new int[] { 1 }), "Expected '' to be equal to '1' but it has less items when using 'System.Collections.Generic.List`1[System.Int32].GetEnumerator()'." },
                { new List<int>(new int[] { 1 }), new List<int>(new int[] { }), "Expected '1' to be equal to '' but it has more items when using 'System.Collections.Generic.List`1[System.Int32].GetEnumerator()'." },
                { new List<int>(new int[] { 1, 2 }), new List<int>(new int[] { 1, 2, 3 }), "Expected '1, 2' to be equal to '1, 2, 3' but it has less items when using 'System.Collections.Generic.List`1[System.Int32].GetEnumerator()'." },
                { new List<int>(new int[] { 1, 2, 3 }), new List<int>(new int[] { 1, 2 }), "Expected '1, 2, 3' to be equal to '1, 2' but it has more items when using 'System.Collections.Generic.List`1[System.Int32].GetEnumerator()'." },
                { new List<int>(new int[] { 1, 2, 3 }), new List<int>(new int[] { 0, 2, 3 }), "Expected '1, 2, 3' to be equal to '0, 2, 3' but it differs at index 0 when using 'System.Collections.Generic.List`1[System.Int32].GetEnumerator()'." },
                { new List<int>(new int[] { 1, 2, 3 }), new List<int>(new int[] { 1, 0, 3 }), "Expected '1, 2, 3' to be equal to '1, 0, 3' but it differs at index 1 when using 'System.Collections.Generic.List`1[System.Int32].GetEnumerator()'." },
                { new List<int>(new int[] { 1, 2, 3 }), new List<int>(new int[] { 1, 2, 0 }), "Expected '1, 2, 3' to be equal to '1, 2, 0' but it differs at index 2 when using 'System.Collections.Generic.List`1[System.Int32].GetEnumerator()'." },
            };

        [Theory]
        [MemberData(nameof(NotEqualData))]
        public void BeEqualTo_With_NotNull_And_NotEqual_Should_Throw(List<int> actual, List<int> expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<List<int>, IEnumerable<int>>>(action);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<List<int>, List<int>, string> NotEqualNullData =>
            new TheoryData<List<int>, List<int>, string> 
            {
                { null, new List<int>(), "Expected '' but found '<null>'." },
                { new List<int>(), null, "Expected '<null>' but found ''." },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Null_Should_Throw(List<int> actual, List<int> expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<List<int>, IEnumerable<int>>>(action);
            Assert.Equal(message, exception.Message);
        }
    }
}
