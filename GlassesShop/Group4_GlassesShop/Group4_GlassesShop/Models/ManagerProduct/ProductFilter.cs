using Group4_GlassesShop.Models.Common;
using Group4_GlassesShop.Models.DataModel;
using LinqKit;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Group4_GlassesShop.Models.ManagerProduct
{
    public class ProductFilter : BaseFilter<Product>
    {
        public int? CategoryId { get ; set; }   

        public int? BrandId { get; set; }

        public int? ColorId { get; set; }

        private string? _search;

        public string? Search { 
            get => _search;
            set => _search = value?.ToLower().Trim();
        }

        public string Sort { get; set; } = "created-desc";

        public override Expression<Func<Product, bool>> GetExpressions()
        {
            var expression = PredicateBuilder.New<Product>(true);
            if (CategoryId != null)
            {
                expression = expression.And(p => p.CategoryId == CategoryId);
            }

            if (BrandId != null)
            {
                expression = expression.And(p => p.BrandId == BrandId);
            }

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var queryExpression = PredicateBuilder.New<Product>();
                queryExpression = queryExpression.Or(p => p.ProducctName.ToLower().Contains(Search));
                expression = expression.And(queryExpression);
            }
            if (ColorId != null)
            {
                expression = expression.And(prod => prod.Stocks.FirstOrDefault(color => color.ColorId == ColorId) != null);
            }

            return expression;
        }
    }
}
