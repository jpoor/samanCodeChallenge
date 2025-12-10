namespace Saman.Backend.Share.shareClasses
{
    public class objList<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public List<T> Items { get; set; }

        public objList(List<T> items, int count, int pageNumber = 1, int pageSize = 10)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            Items = items;
        }

        public static IQueryable<T> ExecuteConditionAndOrders(IQueryable<T>? source, objFiltering? filtering = null)
        {
            if (source is not null
             && filtering is not null)
            {
                // check conditions
                if (filtering.Conditions is not null)
                    foreach (var condition in filtering.Conditions)
                        source = source
                                .Where(condition.getExpression<T>())
                                .AsQueryable();

                // check ordering
                if (filtering.Orders is not null)
                    foreach (var order in filtering.Orders)
                        source = order
                                .getOrdering(source)
                                .AsQueryable();
            }

            return source
                ?? Enumerable.Empty<T>().AsQueryable();
        }

        public static objList<T> ToPagedList(IQueryable<T>? source, objFiltering? filtering = null, bool calculateCount = false)
        {
            // check conditions
            source = ExecuteConditionAndOrders(source, filtering);

            // prepairing
            if (filtering is null) { filtering = new objFiltering(); }
            if (filtering.PageSize > 100) { filtering.PageSize = 100; }

            // check and return empty list
            if (source is null) { return new objList<T>(new List<T>(), 0, filtering.PageNumber, filtering.PageSize); }

            var count = (calculateCount) ? source.Count() : 0;
            var resultItems = source
                             .Skip((filtering.PageNumber - 1) * filtering.PageSize)
                             .Take(filtering.PageSize)
                             .ToList();

            return new objList<T>(resultItems, count, filtering.PageNumber, filtering.PageSize);
        }
    }
}
