using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    //[DebuggerNonUserCode]
    public class StringAssertions
        : ReferenceTypeAssertionsBase<string>
    {
        internal StringAssertions(string? actual)
            : base(actual)
        {
        }

        public StringAssertions BeNull()
            => BeNull(this);

        public StringAssertions BeNotNull()
            => BeNotNull(this);

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

        public StringAssertions BeSameAs<TOther>(TOther other)
            => BeSameAs(this, other);

        public StringAssertions BeNotSameAs<TOther>(TOther other)
            => BeNotSameAs(this, other);

        public StringAssertions EvaluateTrue(Func<string?, bool> func)
            => EvaluateTrue(this, func);

        public StringAssertions EvaluateFalse(Func<string?, bool> func)
            => EvaluateFalse(this, func);

        public StringAssertions BeEqualTo(string? expected)
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

                if (!Actual.Compare(expected, out var index))
                    throw new StringEqualToAssertionException(Actual, expected, index);
            }

            return this;
        }

        public StringAssertions BeNotEqualTo(string? expected)
            => BeNotEqualTo(this, expected);

        public StringAssertions EndWith(string? value)
            => Actual?.EndsWith(value) ?? false
                ? throw new EqualToAssertionException<string, string>(Actual, null)
                : this;
    }
}