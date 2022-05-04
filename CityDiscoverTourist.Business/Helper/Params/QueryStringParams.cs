namespace CityDiscoverTourist.Business.Helper.Params
{
    public abstract class QueryStringParams
    {
        private const int MaxPageSize = 100;

        public int PageNume { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }

        public string? OrderBy { get; set; }
        //public string? Search { get; set; }
    }
}