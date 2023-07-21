using Group4_GlassesShop.Areas.Admin.Models.Repository;
using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Areas.Admin.Models.Interface
{
	public class RepositoryOrder : IRepositoryOrder
	{
		private readonly GlassesShopContext _context;

		public RepositoryOrder(GlassesShopContext context)
		{
			_context = context;
		}
		#region ReportOrder
		/// <summary>
		/// Tong Loi Nhuan GetProfit
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public decimal GetProfit(DateTime startDate, DateTime endDate)
		{
			var orders = _context.Orders.Where(o => o.OrderDate >= startDate &&
							o.OrderDate <= endDate)
							.ToList();
			decimal profit = 0;
			foreach (var order in orders)
			{
				profit += order.TotalPrice;
			}
			return profit;
		}

		public List<OrderModel> GetOrderPainding(DateTime startDate, DateTime endDate)
		{
			var orderPainding = _context.Orders
				.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.StatusId == 1)
				.OrderByDescending(o => o.OrderDate)
				.Select(o => new OrderModel
				{
					OrderId = o.OrderId,
					CustomerId = o.CustomerId,
					OrderDate = o.OrderDate,
					StatusId = o.StatusId,
					AddressId = o.AddressId,
					TotalPrice = o.TotalPrice
				})
				.ToList();

			return orderPainding;
		}


		public int GetTotalOrders(DateTime startDate, DateTime endDate)
		{
			int totalOrders = _context.Orders
				.Count(o => o.OrderDate >= startDate && o.OrderDate <= endDate);

			return totalOrders;
		}

		public int GetTotalOrdersInProgress(DateTime startDate, DateTime endDate)
		{
			int totalOrdersInProgress = _context.Orders
							.Count(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.StatusId == 1);
			return totalOrdersInProgress;
		}

		public decimal GetTotalRevenue(DateTime startDate, DateTime endDate)
		{
			decimal totalRevenue = _context.Orders
					.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.StatusId == 3)
					.Sum(o => o.TotalPrice);
			return totalRevenue;
		}


		public int GetTotalSoldOutProducts(DateTime startDate, DateTime endDate)
		{
			int totalSoldOutProducts = _context.Stocks
				.Join(_context.OrderDetails, s => s.StockId, od => od.StockId, (s, od) => new { Stock = s, OrderDetail = od })
				.Join(_context.Orders, od => od.OrderDetail.OrderId, o => o.OrderId, (od, o) => new { StockOrderDetail = od, Order = o })
				.Where(x => x.Order.OrderDate.Date >= startDate.Date && x.Order.OrderDate.Date <= endDate.Date && x.StockOrderDetail.Stock.Quantity == 0)
				.Count();
			return totalSoldOutProducts;
		}
		public Dictionary<string, decimal> GetRevenueByMonth(DateTime startDate, DateTime endDate)
		{
			var revenueByMonth = _context.Orders
				.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.StatusId == 3)
				.GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
				.Select(g => new
				{
					Month = $"{g.Key.Year}-{g.Key.Month}",
					Revenue = g.Sum(o => o.TotalPrice)
				})
				.ToDictionary(x => x.Month, x => x.Revenue);

			return revenueByMonth;
		}
		public void Confirm(int orderId)
		{
			var Orderselect = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
			if (Orderselect != null && Orderselect.StatusId == 1)
			{
				Orderselect.StatusId = 2;
			}
			_context.SaveChanges();
		}
		#endregion
	}
}
