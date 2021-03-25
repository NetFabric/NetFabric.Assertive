using System;
using System.Text.RegularExpressions;

namespace NetFabric.Assertive
{
    static class StringExtensions
    {
        public static (int Line, int Character) IndexToLineCharacter(this string str, int index)
        {
            var matches = Regex.Matches(str.Substring(0, index), Environment.NewLine);
            return matches switch
            {
                {Count: 0} => (1, index + 1),
                _ => (matches.Count + 1, index - matches[matches.Count - 1].Index - Environment.NewLine.Length + 1)
            };
        }
    }
}
