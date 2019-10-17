using System;
using System.Reactive.Linq;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ReferenceTypeAssertionsTests
    {
        [Fact]
        public void BeObservable_With_Observable_Should_NotThrow()
        {
            // Arrange
            var actual = Observable.Range(0, 0);

            // Act
            actual.Must().BeObservableOf<int>();

            // Assert
        }
    }
}
