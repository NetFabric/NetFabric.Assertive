using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class NullableStringAssertions
        : NullableReferenceTypeAssertionsBase<NullableStringAssertions, string?>
    {
        internal NullableStringAssertions(string? actual)
            : base(actual)
        {
        }

        public NullableStringAssertions BeNullOrEmpty()
            => Actual is null or {Length:0}
                ? this
                : throw new ActualAssertionException<string?>(Actual, Resource.BeNullOrEmptyMessage);

        public NullableStringAssertions BeNotNullOrEmpty()
            => Actual is null or {Length:0}
                ? throw new ActualAssertionException<string?>(Actual, Resource.NotBeNullOrEmptyMessage)
                : this;

        public NullableStringAssertions BeNullOrWhitespace()
            => string.IsNullOrWhiteSpace(Actual)
                ? this
                : throw new ActualAssertionException<string?>(Actual, Resource.BeNullOrWhitespaceMessage);

        public NullableStringAssertions BeNotNullOrWhitespace()
            => string.IsNullOrWhiteSpace(Actual)
                ? throw new ActualAssertionException<string?>(Actual, Resource.NotBeNullOrWhitespaceMessage)
                : this;

        public NullableStringAssertions BeEqualTo(string? expected, bool ignoreCase = false)
        {
            if (Actual is null)
            {
                if (expected is not null)
                    throw new EqualToAssertionException<string?, string?>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<string?, string?>(Actual, expected);

                var (equal, index) = Actual.Compare(expected, ignoreCase);
                if (!equal)
                    throw StringEqualToAssertionException.Create(Actual, expected, index);
            }

            return this;
        }
    }
}