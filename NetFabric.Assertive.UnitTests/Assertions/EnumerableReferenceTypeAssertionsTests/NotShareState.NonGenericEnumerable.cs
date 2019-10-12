using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeNonGenericEnumerable> NonGenericEnumerable_NotSharingStateData =>
            new TheoryData<RangeNonGenericEnumerable>
            {
                { new RangeNonGenericEnumerable(1, 1) },
                { new RangeNonGenericEnumerable(3, 3) },
                { new RangeGenericEnumerable(3, 3, 3) },
            };

        [Theory]
        [MemberData(nameof(NonGenericEnumerable_NotSharingStateData))]
        public void NonGenericEnumerable_NotShareState_With_NotSharingState_Should_NotThrow(RangeNonGenericEnumerable actual)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().NotShareState();

            // Assert
        }

        public static TheoryData<SharingStateRangeNonGenericEnumerable, string> NonGenericEnumerable_SharingStateData =>
            new TheoryData<SharingStateRangeNonGenericEnumerable, string>
            {
                { new SharingStateRangeNonGenericEnumerable(1), "Enumerators returned by 'System.Collections.IEnumerable.GetEnumerator()' do share state." },
                { new SharingStateRangeNonGenericEnumerable(3), "Enumerators returned by 'System.Collections.IEnumerable.GetEnumerator()' do share state." },
            };

        [Theory]
        [MemberData(nameof(NonGenericEnumerable_SharingStateData))]
        public void NonGenericEnumerable_NotShareState_With_SharingState_Should_Throw(SharingStateRangeNonGenericEnumerable actual, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().NotShareState();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<SharingStateRangeNonGenericEnumerable>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Equal(message, exception.Message);
        }
    }
}
