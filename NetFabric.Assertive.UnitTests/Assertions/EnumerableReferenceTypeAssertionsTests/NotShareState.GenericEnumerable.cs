using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeGenericEnumerable> GenericEnumerable_NotSharingStateData =>
            new TheoryData<RangeGenericEnumerable>
            {
                { new RangeGenericEnumerable(1, 1, 1) },
                { new RangeGenericEnumerable(3, 3, 3) },
            };

        [Theory]
        [MemberData(nameof(GenericEnumerable_NotSharingStateData))]
        public void GenericEnumerable_NotShareState_With_NotSharingState_Should_NotThrow(RangeGenericEnumerable actual)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().NotShareState();

            // Assert
        }

        public static TheoryData<SharingStateRangeGenericEnumerable, string> GenericEnumerable_SharingStateData =>
            new TheoryData<SharingStateRangeGenericEnumerable, string>
            {
                { new SharingStateRangeGenericEnumerable(1), "Enumerators returned by 'System.Collections.IEnumerable`1[System.Int32].GetEnumerator()' do share state." },
                { new SharingStateRangeGenericEnumerable(3), "Enumerators returned by 'System.Collections.IEnumerable`1[System.Int32].GetEnumerator()' do share state." },
            };

        [Theory]
        [MemberData(nameof(GenericEnumerable_SharingStateData))]
        public void GenericEnumerable_NotShareState_With_SharingState_Should_Throw(SharingStateRangeGenericEnumerable actual, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().NotShareState();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<SharingStateRangeGenericEnumerable>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Equal(message, exception.Message);
        }
    }
}
