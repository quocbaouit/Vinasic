using System;

namespace Dynamic.Framework.Infrastructure
{
    public class PagingInput
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public SortDirection SortDirection { get; set; }

        public string SortPropertyName { get; set; }

        public static string GetSortPropertyName(string sorting)
        {
            int length = sorting.LastIndexOf(" ", StringComparison.Ordinal);
            string str = sorting.Substring(0, length);
            sorting.Substring(length + 1, sorting.Length - length - 1);
            return str;
        }

        public static SortDirection GetDirection(string sorting)
        {
            int length = sorting.LastIndexOf(" ", StringComparison.Ordinal);
            sorting.Substring(0, length);
            return sorting.Substring(length + 1, sorting.Length - length - 1).ToLower().Equals("asc") ? SortDirection.Ascending : SortDirection.Descending;
        }
    }
}
