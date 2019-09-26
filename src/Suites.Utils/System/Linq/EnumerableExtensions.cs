using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static bool SetEquals<T>(this IEnumerable<T> first, in IEnumerable<T> second) => first.SetEquals(second, null);

        public static bool SetEquals<T>(this IEnumerable<T> first, in IEnumerable<T> second, IEqualityComparer<T> comparer)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            NamedNullException.Assert(first, nameof(first));
            NamedNullException.Assert(second, nameof(second));
            var _fisrt = new HashSet<T>(first, comparer);
            return _fisrt.SetEquals(second);
        }

        /// <summary>
        ///     Enumerates the sequence and invokes the given action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        private static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (onNext == null)
            {
                throw new ArgumentNullException("onNext");
            }
            foreach (var item in source)
            {
                onNext(item);
            }
        }

        public static (IEnumerable<T> FirstPart, IEnumerable<T> SecondPart) Split2Parts<T>(this IEnumerable<T> items,
            Predicate<T> firstPartJudger)
        {
            NamedNullException.Assert(items, nameof(items));
            NamedNullException.Assert(firstPartJudger, nameof(firstPartJudger));

            var first = new List<T>();
            var second = new List<T>();

            items.ForEach(x =>
            {
                if (firstPartJudger(x))
                {
                    first.Add(x);
                }
                else
                {
                    second.Add(x);
                }
            });

            return (first, second);
        }

        public static T[] Try2Array<T>(this IEnumerable<T> items)
        {
            NamedNullException.Assert(items, nameof(items));

            return items is T[] array ? array : items.ToArray();
        }

        private static (IEnumerable<T> removes, IEnumerable<T> adds) GetCollectionSyncInfo<T>(
            this IEnumerable<T> from,
            IEnumerable<T> to,
            IEqualityComparer<T> comparer)
        {
            var r = new List<T>();
            var a = new List<T>();

            var fromSet = new HashSet<T>(from, comparer);
            foreach (var t in to)
            {
                if (!fromSet.Contains(t))
                {
                    r.Add(t);
                }
            }

            var toSet = new HashSet<T>(to, comparer);
            foreach (var f in from)
            {
                if (!toSet.Contains(f))
                {
                    a.Add(f);
                }
            }

            return (r, a);
        }

        /// <summary>
        /// 同步两个集合，使`to`最终与`from`集合一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from">同步源</param>
        /// <param name="to">同步目标</param>
        /// <param name="comparer"></param>
        /// <returns>返回目标</returns>
        public static IEnumerable<T> SyncTo<T>(
            this IEnumerable<T> from,
            ICollection<T> to,
            IEqualityComparer<T> comparer = default)
        {
            NamedNullException.Assert(from, nameof(from));
            NamedNullException.Assert(to, nameof(to));

            if (comparer == default)
            {
                comparer = EqualityComparer<T>.Default;
            }

            var (r, a) = from.GetCollectionSyncInfo(to, comparer);
            r.ForEach(x => to.Remove(x));
            a.ForEach(x => to.Add(x));
            return to;
        }

        [Obsolete("使用`EnumerableEx`的`Expand`扩展方法")]
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            if (source == default || !source.Any())
            {
                return Enumerable.Empty<T>();
            }

            var part2 = source.Where(x => selector(x) != default)
                .SelectMany(x => selector(x))
                .ToArray()
                .Flatten(selector);
            return source.Concat(part2).ToArray();
        }

        public static IEnumerable<T> NullSafe<T>(this IEnumerable<T> source)
            => source ?? Enumerable.Empty<T>();

        public static IEnumerable<T> SkipNull<T>(this IEnumerable<T> source)
            => source.Where(x => x != default).ToArray();

        public static IEnumerable<T> ToEnumerable<T>(this T obj)
            => obj == default ? Enumerable.Empty<T>() : new[] { obj };

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
            => source == default || !source.Any();
    }
}
