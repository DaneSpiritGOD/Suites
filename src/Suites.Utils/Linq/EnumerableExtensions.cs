using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static bool SetEquals<T>(this IEnumerable<T> first, in IEnumerable<T> second)
        {
            return first.SetEquals(second,null);
        }

        public static bool SetEquals<T>(this IEnumerable<T> first, in IEnumerable<T> second, IEqualityComparer<T> comparer)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            NamedNullException.Assert(first, nameof(first));
            NamedNullException.Assert(second, nameof(second));
            var _fisrt = new HashSet<T>(first, comparer);
            return _fisrt.SetEquals(second);
        }
    }
}
