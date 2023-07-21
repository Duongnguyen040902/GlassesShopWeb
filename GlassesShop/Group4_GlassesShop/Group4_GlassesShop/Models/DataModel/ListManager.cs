using Group4_GlassesShop.Models.ManagerProduct;
using X.PagedList;
using X.PagedList.Mvc.Core;
namespace Group4_GlassesShop.Models.DataModel
{
    public class ListManager
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }

        public List<Brand> Brand { get; set; }
        public List<Color> Color { get; set; }

        public PagedList<Product> PageList { get;  set; }

        public ProductFilter Filter { get; set; } = null!;
    }
}
