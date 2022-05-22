namespace CityDiscoverTourist.Business.Helper
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public IEnumerable<T> Data { get; set; }

        public PageList(List<T> items, int count, int pageNum, int pageSize)
        {
            Data = items;
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNum;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PageList<T> ToPageList(IEnumerable<T> source, int pageNum, int pageSize)
        {
            var count = source.Count();
            var item = source.Skip((pageNum - 1) * pageSize)
                .Take(pageSize).ToList();

            return new PageList<T>(item, count, pageNum, pageSize);
        }
    }
}