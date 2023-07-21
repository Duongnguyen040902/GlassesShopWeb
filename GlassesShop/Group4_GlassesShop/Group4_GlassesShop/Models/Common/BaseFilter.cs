using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Group4_GlassesShop.Models.Common
{
    public abstract class BaseFilter<T> where T : class
    {
        public string SortDir { get; set; } = "asc";

        public string? SortBy { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 12;

        public abstract Expression<Func<T, bool>> GetExpressions();

        public Func<IQueryable<T>, IOrderedQueryable<T>>? GetOrder()
        {
            if (string.IsNullOrWhiteSpace(SortBy)) return null;

            return query => query.OrderBy($"{SortBy} {SortDir.ToString().ToLower()}");
        }
    }
}
