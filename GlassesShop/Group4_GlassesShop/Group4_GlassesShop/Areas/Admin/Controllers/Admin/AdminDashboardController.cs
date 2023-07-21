using Group4_GlassesShop.Areas.Admin.Models;
using Group4_GlassesShop.Areas.Admin.Models.Interface;
using Group4_GlassesShop.Areas.Admin.Models.Repository;
using Group4_GlassesShop.Models.Authentication;
using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace Group4_GlassesShop.Controllers
{
	[Area("admin")]
	[Route("admin")]
	[Route("admin/adminDashboard")]

	public class AdminDashboardController : Controller
	{
		private IRepositoryOrder _repositoryOrder;

		public AdminDashboardController(IRepositoryOrder repositoryOrder)
		{
			_repositoryOrder = repositoryOrder;

		}
        #region Dashboard
        [AuthenAdmin]
        [Route("")]
		[Route("dashboard")]
		[HttpGet]
		public IActionResult Dashboard(DateTime startDate, DateTime endDate, string reportType)
		{
			startDate = DateTime.SpecifyKind(DateTime.Today.Date, DateTimeKind.Utc); // Set the time zone and add 7 hours (according to Vietnam time zone)
			endDate = startDate.AddDays(1).AddSeconds(-1); // End at 23:59:59 according to Vietnam time zone
			decimal totalRevenue = _repositoryOrder.GetTotalRevenue(startDate, endDate);
			int soldOutProducts = _repositoryOrder.GetTotalSoldOutProducts(startDate, endDate);
			int totalOrders = _repositoryOrder.GetTotalOrders(startDate, endDate);
			int totalOrderInPro = _repositoryOrder.GetTotalOrdersInProgress(startDate, endDate);
			var RevenueByMonth = _repositoryOrder.GetRevenueByMonth(startDate,endDate);
			ReportOrder reportOrder = new ReportOrder(totalRevenue, totalOrders, totalOrderInPro, soldOutProducts, RevenueByMonth);
			var orderPainding = _repositoryOrder.GetOrderPainding(startDate, endDate);
			var dailyReport = new ReportDetail
			{
				ReportOrder = reportOrder,
				Order = orderPainding,
			};
			return View(dailyReport);
		}

        [AuthenAdmin]
        [Route("")]
		[Route("dashboard")]
		[HttpPost]
		public IActionResult Dashboard(DateTime fromDate, DateTime toDate)
		{
			decimal totalRevenue = _repositoryOrder.GetTotalRevenue(fromDate, toDate);
			int soldOutProducts = _repositoryOrder.GetTotalSoldOutProducts(fromDate, toDate);
			int totalOrders = _repositoryOrder.GetTotalOrders(fromDate, toDate);
			int totalOrderInPro = _repositoryOrder.GetTotalOrdersInProgress(fromDate, toDate);
			var RevenueByMonth = _repositoryOrder.GetRevenueByMonth(fromDate, toDate);
			ReportOrder reportOrder = new ReportOrder(totalRevenue, totalOrders, totalOrderInPro, soldOutProducts, RevenueByMonth);
			var orderPainding = _repositoryOrder.GetOrderPainding(fromDate, toDate);
			var Report = new ReportDetail
			{
				ReportOrder = reportOrder,
				Order = orderPainding,
			};
			return View(Report);
		}
		#endregion

	}
}
