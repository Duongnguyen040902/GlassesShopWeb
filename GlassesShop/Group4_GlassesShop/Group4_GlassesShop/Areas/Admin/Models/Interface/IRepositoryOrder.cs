using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Areas.Admin.Models.Repository
{
	public interface IRepositoryOrder
	{
		decimal GetTotalRevenue(DateTime startDate, DateTime endDate);
		decimal GetProfit(DateTime startDate, DateTime endDate);
		int GetTotalOrders(DateTime startDate, DateTime endDate);
		int GetTotalOrdersInProgress(DateTime startDate, DateTime endDate);
		int GetTotalSoldOutProducts(DateTime startDate, DateTime endDate);

		List<OrderModel> GetOrderPainding(DateTime startDate, DateTime endDate);
		Dictionary<string, decimal> GetRevenueByMonth(DateTime startDate, DateTime endDate);
		void Confirm(int orderId);
	}
}
