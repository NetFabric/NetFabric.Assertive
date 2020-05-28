using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestEnumerable, int[]> Enumerable_EqualData =>
            new TheoryData<TestEnumerable, int[]>
            {
                { null, null },

                { new TestEnumerable(TestData.Empty), TestData.Empty },
                { new TestEnumerable(TestData.Single), TestData.Single },
                { new TestEnumerable(TestData.Multiple), TestData.Multiple },

                { new TestNonGenericEnumerable(TestData.Empty), TestData.Empty },
                { new TestNonGenericEnumerable(TestData.Single), TestData.Single },
                { new TestNonGenericEnumerable(TestData.Multiple), TestData.Multiple },

                { new TestGenericEnumerable(TestData.Empty), TestData.Empty },
                { new TestGenericEnumerable(TestData.Single), TestData.Single },
                { new TestGenericEnumerable(TestData.Multiple), TestData.Multiple },

                { new TestCollection(TestData.Empty), TestData.Empty },
                { new TestCollection(TestData.Single), TestData.Single },
                { new TestCollection(TestData.Multiple), TestData.Multiple },

                { new TestList(TestData.Empty), TestData.Empty },
                { new TestList(TestData.Single), TestData.Single },
                { new TestList(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(Enumerable_EqualData))]
        public void BeEqualTo_With_Equal_Should_NotThrow(TestEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }
    }
}