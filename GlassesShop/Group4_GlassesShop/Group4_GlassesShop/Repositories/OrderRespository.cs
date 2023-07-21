using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;

namespace Group4_GlassesShop.Repositories
{

    public class OrderRespository : IOrderRepository
    {
        private readonly GlassesShopContext _dbContext;

        public OrderRespository(GlassesShopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            return _dbContext.Orders.Where(o => o.CustomerId == customerId).ToList();
        }
        public List<Order> GetOrderByOrderIdAndCustomerId(string searchKey, int customerId)
        {

            return _dbContext.Orders.Where(o => o.OrderId == int.Parse(searchKey) && o.CustomerId == customerId).ToList();
        }
        public List<Order> GetOrderByProdcutNameAndCustomerId(string searchKey, int customerId)
        {
            return _dbContext.Orders
                .Join(_dbContext.OrderDetails, o => o.OrderId, od => od.OrderId, (o, od) => new { Order = o, OrderDetail = od })
                .Join(_dbContext.Stocks, od => od.OrderDetail.StockId, s => s.StockId, (od, s) => new { od.Order, od.OrderDetail, Stock = s })
                .Join(_dbContext.Products, s => s.Stock.ProductId, p => p.Pid, (s, p) => new { s.Order, s.OrderDetail, s.Stock, Product = p })
                .Where(joined => joined.Product.ProducctName.Contains(searchKey) && joined.Order.CustomerId == customerId)
                .Select(joined => joined.Order)
                .ToList();
        }
        public Product GetProductById(int productId)
        {
            return _dbContext.Products.FirstOrDefault(p => p.Pid == productId);
        }

    }
}