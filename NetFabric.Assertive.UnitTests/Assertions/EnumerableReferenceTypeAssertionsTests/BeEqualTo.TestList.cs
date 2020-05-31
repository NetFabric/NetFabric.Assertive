using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestList, int[]> BeEqualTo_TestList_EqualData =>
            new TheoryData<TestList, int[]>
            {
                { new TestList(TestData.Empty),    TestData.Empty },
                { new TestList(TestData.Single),   TestData.Single },
                { new TestList(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestList_EqualData))]
        public void BeEqualTo_TestList_With_Equal_Should_NotThrow(TestList actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }
    }
}