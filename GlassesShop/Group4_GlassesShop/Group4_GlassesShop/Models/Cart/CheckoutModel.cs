using Group4_GlassesShop.Models.AddresManager;
using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Models.Cart
{
    public class CheckoutModel
    {
        public Cart ?Carts { get; set; } = new Cart();

        public Customer Customer { get; set; } = new Customer();

        public Order Order { get; set; } = new Order() { StatusId = 1 };

        public List<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();

        public Status Status { get; set; } = new Status();

        public AddressManager? Address { get; set; }
    }
}
