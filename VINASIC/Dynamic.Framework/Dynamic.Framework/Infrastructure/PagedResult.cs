using Dynamic.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynamic.Framework.Infrastructure
{
    public class PagedResult<T>
    {
        [NonSerialized]
        private IQueryable<T> _source;
        private List<T> _sourcePaged;
        private long _rowCount;

        public PagingInput PagingInput { get; set; }

        public List<T> Results
        {
            get
            {
                if (this.CurrentPageIndex == -1)
                    this.CurrentPageIndex = Convert.ToInt32(this.PageCount - 1L);
                return this._sourcePaged ?? (this._sourcePaged = Enumerable.ToList<T>((IEnumerable<T>)Queryable.Take<T>(Queryable.Skip<T>(QueryableExtensions.OrderBy<T>(this._source, this.PagingInput.SortPropertyName, this.PagingInput.SortDirection), this.CurrentPageIndex * this.PageSize), this.PageSize)));
            }
            set
            {
                this._sourcePaged = value;
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                return this.PagingInput.PageIndex;
            }
            set
            {
                this.PagingInput.PageIndex = value;
            }
        }

        public long PageCount
        {
            get
            {
                return (this.RowCount - 1L) / (long)this.PageSize + 1L;
            }
        }

        public int PageSize
        {
            get
            {
                return this.PagingInput.PageSize;
            }
        }

        public long RowCount
        {
            get
            {
                return this._rowCount;
            }
            set
            {
                this._rowCount = value;
            }
        }

        public PagedResult()
        {
        }

        public PagedResult(IQueryable<T> source)
        {
            this._source = source;
        }

        public void SetPaging(PagingInput paging)
        {
            this.RowCount = Queryable.LongCount<T>(this._source);
            this.PagingInput = paging;
            if (this.CurrentPageIndex == -1)
                this.CurrentPageIndex = Convert.ToInt32(this.PageCount - 1L);
            this._sourcePaged = Enumerable.ToList<T>((IEnumerable<T>)Queryable.Take<T>(Queryable.Skip<T>(QueryableExtensions.OrderBy<T>(this._source, this.PagingInput.SortPropertyName, this.PagingInput.SortDirection), this.CurrentPageIndex * this.PageSize), this.PageSize));
        }
    }
}
