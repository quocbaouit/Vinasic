using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dynamic.Framework
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int size) where TSource : class
        {
            EnumerableExtensions.CheckSource((object)source);
            if (page < 0)
                throw new ArgumentOutOfRangeException("page");
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size");
            int index = EnumerableExtensions.GetIndex(page, size);
            return Enumerable.Take<TSource>(Enumerable.Skip<TSource>(source, index), size);
        }

        private static void CheckNullOrEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("propertyName");
        }

        private static void CheckSource(object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        }

        private static int GetIndex(int page, int size)
        {
            return page * size;
        }
    }
}
