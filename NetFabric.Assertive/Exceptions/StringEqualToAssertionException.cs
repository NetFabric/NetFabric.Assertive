using System;
using System.Text;

namespace NetFabric.Assertive
{
    //[DebuggerNonUserCode]
    public class StringEqualToAssertionException
        : ActualAssertionException<string>
    {
        const char ellipses = '\u2026';
        const char pointerUp = '\u25b2';
        const char pointerDown = '\u25bc';
        const char newLine = '\u2193';
        const char carriageReturn = '\u2190';
        const char arrowTab = '\u0362';
        const char bullet = '\u2022';
        const char space = ' ';
        
        public StringEqualToAssertionException(string actual, string expected, int index)
            : this(actual, expected, index, $"Expected to be equal but it's not at position {index}.")
        {
        }

        public StringEqualToAssertionException(string actual, string expected, int index, string message)
            : base(actual, message)
        {
            Index = index;
            Expected = expected;

            var actualMessage   = new StringBuilder("  Actual: ");
            var expectedMessage = new StringBuilder("Expected: ");
            var pointerMessage  = new StringBuilder("          ");

            var startIndex = 0;
            var pointerIndex = index;

            if (index > 10)
            {
                startIndex = index - 10;
                pointerIndex = 10;
                _ = actualMessage.Append(ellipses);
                _ = expectedMessage.Append(ellipses);
                _ = pointerMessage.Append(space);
            }

            var actualString = actual switch
            {
                null => "<null>",
                var str when str == "" => "<empty>",
                _ => ReplaceSpecialCharacters(actual.Substring(startIndex, Math.Min(20, actual.Length - startIndex)))
            };
            var expectedString = expected switch
            {
                null => "<null>",
                var str when str == "" => "<empty>",
                _ => ReplaceSpecialCharacters(expected.Substring(startIndex, Math.Min(20, expected.Length - startIndex)))
            };

            _ = actualMessage.Append(actualString);
            _ = expectedMessage.Append(expectedString);
            _ = pointerMessage.Append(space, pointerIndex).Append(pointerUp);

            if (actual is not null && actual.Length - startIndex > 20)
                _ = actualMessage.Append(ellipses);
            if (expected is not null && expected.Length - startIndex > 20)
                _ = expectedMessage.Append(ellipses);

            Message = $"{message}{Environment.NewLine}{expectedMessage}{Environment.NewLine}{actualMessage}{Environment.NewLine}{pointerMessage}";
        }

        string ReplaceSpecialCharacters(string str)
            => str
            .Replace('\n', newLine)
            .Replace('\r', carriageReturn)
            .Replace('\t', arrowTab)
            .Replace(' ', bullet);

        public override string Message { get; }
        public int Index { get; }
        public string Expected { get; }

        public override string ToString() 
            => Message;
    }
}