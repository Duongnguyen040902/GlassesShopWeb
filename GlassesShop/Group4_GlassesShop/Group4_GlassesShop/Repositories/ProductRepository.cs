using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly GlassesShopContext _context;

        public ProductRepository(GlassesShopContext context) : base(context) 
        {
            _context = context;
        }
    }
}
