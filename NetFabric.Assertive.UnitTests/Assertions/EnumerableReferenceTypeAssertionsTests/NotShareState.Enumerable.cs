using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeEnumerable> Enumerable_NotSharingStateData =>
            new TheoryData<RangeEnumerable>
            {
                { new RangeEnumerable(1) },
                { new RangeEnumerable(3) },
                { new RangeNonGenericEnumerable(3, 3) },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotSharingStateData))]
        public void Enumerable_NotShareState_With_NotSharingState_Should_NotThrow(RangeEnumerable actual)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().NotShareState();

            // Assert
        }

        public static TheoryData<SharingStateRangeEnumerable, string> Enumerable_SharingStateData =>
            new TheoryData<SharingStateRangeEnumerable, string>
            {
                { new SharingStateRangeEnumerable(1), "Enumerators returned by 'NetFabric.Assertive.UnitTests.SharingStateRangeEnumerable.GetEnumerator()' do share state." },
                { new SharingStateRangeEnumerable(3), "Enumerators returned by 'NetFabric.Assertive.UnitTests.SharingStateRangeEnumerable.GetEnumerator()' do share state." },
            };

        [Theory]
        [MemberData(nameof(Enumerable_SharingStateData))]
        public void Enumerable_NotShareState_With_SharingState_Should_Throw(SharingStateRangeEnumerable actual, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().NotShareState();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<SharingStateRangeEnumerable>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Equal(message, exception.Message);
        }
    }
}
