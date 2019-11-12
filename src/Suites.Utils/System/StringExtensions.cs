using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// StringBuilder版本的TrimEnd
        /// </summary>
        /// <param name="stringBuilder"></param>
        public static void TrimEnd(this StringBuilder stringBuilder)
        {
            NamedNullException.Assert(stringBuilder, nameof(stringBuilder));

            while (stringBuilder.Length - 1 >= 0 && char.IsWhiteSpace(stringBuilder[stringBuilder.Length - 1]))
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
        }

        public static string JoinStringArray(this IEnumerable<string> msgs, int maxLineCount = 10)
            => JoinStringArray(msgs, Environment.NewLine, maxLineCount);

        public static string JoinStringArray(this IEnumerable<string> msgs, string seperator, int maxLineCount = 10)
        {
            if (msgs == default)
            {
                return string.Empty;
            }

            return string.Join(seperator, ConditionalConcat()).Trim();

            IEnumerable<string> ConditionalConcat()
                => (msgs.Count() <= maxLineCount) ? msgs : msgs.Take(maxLineCount).Concat(new[] { "..." });
        }

        public static bool IgnoreCaseEquals(this string name1, string name2)
            => string.Equals(name1, name2, StringComparison.OrdinalIgnoreCase);

        public static (string Part1, string Part2) Split2Parts(this string source, string seperator)
        {
            StringNullOrEmptyException.Assert(source, nameof(source));
            StringNullOrEmptyException.Assert(seperator, nameof(seperator));

            var sepLength = seperator.Length;
            var sepIndex = source.AsSpan().IndexOf(seperator.AsSpan());

            if (sepIndex < 0)
            {
                return (source, string.Empty);
            }

            return (source.Substring(0, sepIndex),
                sepIndex == source.Length - sepLength ? string.Empty : source.Substring(sepIndex + sepLength));
        }
    }
}
