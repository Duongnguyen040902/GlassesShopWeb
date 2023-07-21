using Group4_GlassesShop.Areas.Admin.Models.Repository;
using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Areas.Admin.Models.Interface
{
    public class StockRepository : IStockRepository
    {
        private readonly GlassesShopContext _context;

        public StockRepository(GlassesShopContext context)
        {
            _context = context;
        }
        public StockModel AddStock(StockModel stock, int ProductId, int quantity, int ColorId)
        {
            var existingStock = _context.Stocks.FirstOrDefault(s => s.ProductId == ProductId && s.ColorId == ColorId);
            if (existingStock != null)
            {
                existingStock.Quantity += quantity;
                _context.SaveChanges();
                return new StockModel
                {
                    ColorId = ColorId,
                    ProductId = ProductId,
                    Quantity = existingStock.Quantity,
                };
            }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.Pid == ProductId);
                if (product != null)
                {
                    var newStock = new Stock
                    {
                        ColorId = ColorId,
                        ProductId = ProductId,
                        Quantity = quantity,
                    };
                    _context.Stocks.Add(newStock);
                    _context.SaveChanges();
                    return new StockModel
                    {
                        ColorId = ColorId,
                        ProductId = ProductId,
                        Quantity = quantity,
                    };
                }
            }
            return null;
        }


        public void DeleteStock(int id)
        {
            throw new NotImplementedException();
        }

        public List<StockModel> getAllStock()
        {
            throw new NotImplementedException();
            //var getAllStock = _context.Stocks
            //    .OrderBy(s => s.ProductId)
            //    .ThenBy(s => s.StockId)
            //    .Select(s => new StockModel
            //    {
            //        StockId = s.StockId,
            //        ColorId = s.ColorId,
            //        ProductId = s.ProductId,
            //        Quantity = s.Quantity
            //    });

            //return getAllStock.ToList();
        }

        public List<StockModel> getAllStockById(int pid)
        {
            var getAllStock = _context.Stocks.Where(s => s.ProductId == pid).Select(x => new StockModel
            {
                ColorId = x.ColorId,
                ProductId = x.ProductId,
                StockId= x.StockId,
                Quantity = x.Quantity
            }).ToList();
            return getAllStock;
        }

        public StockModel getStock(int id)
        {
            var getStockById = _context.Stocks.SingleOrDefault(s => s.StockId == id);
            if (getStockById != null)
            {
                return new StockModel
                {
                    StockId = getStockById.StockId,
                    ColorId = getStockById.ColorId,
                    ProductId = getStockById.ProductId,
                    Quantity = getStockById.Quantity
                };
            }
            return null;
        }

        public void UpdateStock(StockModel stock)
        {
            throw new NotImplementedException();
        }
    }
}
