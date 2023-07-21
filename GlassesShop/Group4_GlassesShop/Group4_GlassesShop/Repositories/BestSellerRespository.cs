using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;

namespace Group4_GlassesShop.Repositories
{
    public class BestSellerRespository : IBestSellerRespository

    {
        private readonly GlassesShopContext _context;

        public BestSellerRespository(GlassesShopContext dbContext)
        {
            _context = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateBestSellers()
        {

            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1); // Get the date one month ago from the current date

            // Query the database to find products that have been best sellers in the last one month
            var bestSellersQuery = (from order in _context.Orders
                                    from orderDetail in order.OrderDetails
                                    join stock in _context.Stocks on orderDetail.StockId equals stock.StockId
                                    join product in _context.Products on stock.ProductId equals product.Pid
                                    where order.OrderDate >= oneMonthAgo && order.StatusId == 3
                                    group orderDetail by product.Pid into productGroup
                                    where productGroup.Sum(od => od.Quantity) > 5
                                    select new
                                    {
                                        ProductId = productGroup.Key,
                                        TotalSoldQuantity = productGroup.Sum(od => od.Quantity)
                                    })
                                   .ToList();

            // Filter the products that have a total sold quantity greater than 5 and order them by the total sold quantity in descending order. Take the top 5 best-selling products.
            var productsWithQuantityGreaterThan5 = bestSellersQuery
                                                    .Where(bs => bs.TotalSoldQuantity > 5)
                                                    .OrderByDescending(bs => bs.TotalSoldQuantity)
                                                    .Take(5)
                                                    .ToList();

            
            var productIdsToUpdate = productsWithQuantityGreaterThan5.Select(bs => bs.ProductId).ToList();// Extract the product IDs of the best-selling products to update

            // Fetch the products from the database that are identified as best sellers
            var productsToUpdate = _context.Products
                                     .Where(p => productIdsToUpdate.Contains(p.Pid))
                                     .ToList();

            // Set the BestSeller property of the identified best-selling products to true
            foreach (var product in productsToUpdate)
            {
                product.BestSeller = true;
            }
            _context.SaveChanges(); 
        }
    }
}
