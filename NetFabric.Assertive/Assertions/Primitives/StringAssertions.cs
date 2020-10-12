using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class StringAssertions
        : ReferenceTypeAssertionsBase<StringAssertions, string>
    {
        internal StringAssertions(string? actual)
            : base(actual)
        {
        }

        public StringAssertions BeNullOrEmpty()
            => string.IsNullOrEmpty(Actual)
                ? this
                : throw new ActualAssertionException<string>(Actual, Resource.BeNullOrEmptyMessage);

        public StringAssertions BeNotNullOrEmpty()
            => string.IsNullOrEmpty(Actual)
                ? throw new ActualAssertionException<string>(Actual, Resource.NotBeNullOrEmptyMessage)
                : this;

        public StringAssertions BeNullOrWhitespace()
            => string.IsNullOrWhiteSpace(Actual)
                ? this
                : throw new ActualAssertionException<string>(Actual, Resource.BeNullOrWhitespaceMessage);

        public StringAssertions BeNotNullOrWhitespace()
            => string.IsNullOrWhiteSpace(Actual)
                ? throw new ActualAssertionException<string>(Actual, Resource.NotBeNullOrWhitespaceMessage)
                : this;

        public StringAssertions BeEqualTo(string? expected, bool ignoreCase = false)
        {
            if (Actual is null)
            {
                if (expected is not null)
                    throw new EqualToAssertionException<string, string>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<string, string>(Actual, expected);

                var (equal, index) = Actual.Compare(expected, ignoreCase);
                if (!equal)
                    throw StringEqualToAssertionException.Create(Actual, expected, index);
            }

            return this;
        }

        public StringAssertions EndWith(string? value)
            => Actual?.EndsWith(value) ?? false
                ? throw new EqualToAssertionException<string, string>(Actual, null)
                : this;
    }
}