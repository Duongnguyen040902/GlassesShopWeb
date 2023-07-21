namespace Group4_GlassesShop.Areas.Admin.Models
{
    public class OrderModel
    {
		public int OrderId { get; set; }
		public int CustomerId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalPrice { get; set; }
		public int AddressId { get; set; }
		public int? StatusId { get; set; }
    }
}
