using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Models.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetOrdersByCustomerId(int customerId);
        List<Order> GetOrderByOrderIdAndCustomerId(string searchKey, int customerId);
        List<Order> GetOrderByProdcutNameAndCustomerId(string searchKey, int customerId);
        Product GetProductById(int productId);

    }
}
