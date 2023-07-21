namespace Group4_GlassesShop.Areas.Admin.Models
{
	public class ReportOrder
	{
		public decimal totalRevenue { get; set; }

		public int TotalOrders { get; set; }

		public int TotalOrderInPro { get; set; }

		public int SoldOutProducts { get; set; }
		public Dictionary<string, decimal> RevenueByMonth { get; set; }

		public ReportOrder()
		{

		}

		public ReportOrder(decimal totalRevenue, int totalOrders, int totalOrderInPro, int soldOutProducts, Dictionary<string, decimal> revenueByMonth)
		{
			this.totalRevenue = totalRevenue;
			TotalOrders = totalOrders;
			TotalOrderInPro = totalOrderInPro;
			SoldOutProducts = soldOutProducts;
			RevenueByMonth = revenueByMonth;
		}
	}
}
