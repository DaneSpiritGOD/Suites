using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// 应用在列表上的用于关键词过滤的扩展方法
    /// </summary>
    public static class FilterExtensions
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, string key,
            Func<T, string> contentSelector)
            => items.Filter(key, contentSelector, int.MaxValue);

        public const int LimitFilterCount = 50;
        public static IEnumerable<T> LimitFilter<T>(this IEnumerable<T> items, string key,
            Func<T, string> contentSelector, int limit = LimitFilterCount)
            => items.Filter(key, contentSelector, limit);

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, string key,
            Func<T, bool> judger)
            => items.Filter(key, (item, _) => judger(item), int.MaxValue);

        public static IEnumerable<T> LimitFilter<T>(this IEnumerable<T> items, string key,
            Func<T, bool> judger, int limit = LimitFilterCount)
            => items.Filter(key, (item, _) => judger(item), limit);

        private static bool InternalContainsKey(this string source, string key)
        {
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }

            return source.ToLower().Contains(key.ToLower().Trim());
        }

        public static bool ContainsKey(this string source, string key)
        {
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                return true;
            }

            return source.ToLower().Contains(key.ToLower().Trim());
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, string key,
            Func<T, string> contentSelector,
            int maxDisplayCount)
        {
            NamedNullException.Assert(contentSelector, nameof(contentSelector));

            return items.Filter(key,
                (item, k) => contentSelector(item).InternalContainsKey(k),
                maxDisplayCount);
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, string key,
            IEnumerable<Func<T, string>> propertyToFilters)
            => Filter(items, key, propertyToFilters, int.MaxValue);
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, string key,
            params Func<T, string>[] propertyToFilters)
            => Filter(items, key, propertyToFilters, int.MaxValue);
        public static IEnumerable<T> LimitFilter<T>(this IEnumerable<T> items, string key,
            IEnumerable<Func<T, string>> propertyToFilters,
            int limit = LimitFilterCount)
            => Filter(items, key, propertyToFilters, limit);
        public static IEnumerable<T> LimitFilter<T>(this IEnumerable<T> items, string key,
            int limit, params Func<T, string>[] propertyToFilters)
            => Filter(items, key, propertyToFilters, limit);
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, string key,
            IEnumerable<Func<T, string>> propertyToFilters,
            int maxDisplayCount)
        {
            NamedNullException.Assert(propertyToFilters, nameof(propertyToFilters));

            return items.Filter(key,
                (item, k) => propertyToFilters.Any(p => p(item).InternalContainsKey(k)),
                maxDisplayCount);
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, string key,
            Func<T, string, bool> judger,
            int maxDisplayCount)
        {
            NamedNullException.Assert(items, nameof(items));
            NamedNullException.Assert(judger, nameof(judger));

            maxDisplayCount = maxDisplayCount > 0 ? maxDisplayCount : int.MaxValue;

            return string.IsNullOrWhiteSpace(key) ? items.Take(maxDisplayCount).ToArray() :
                items.Where(x => judger(x, key))
                .Take(maxDisplayCount)
                .ToArray();
        }
    }
}
