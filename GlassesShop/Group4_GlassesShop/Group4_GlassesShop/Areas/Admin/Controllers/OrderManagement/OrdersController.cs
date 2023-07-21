using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group4_GlassesShop.Models.DataModel;
using System.Drawing.Printing;
using X.PagedList;
using Group4_GlassesShop.Models.Authentication;
using static NuGet.Packaging.PackagingConstants;
using Group4_GlassesShop.Models.Interface;
using Group4_GlassesShop.Repositories;

namespace Group4_GlassesShop.Controllers
{

    [AuthenAdmin]
	[Area("admin")]
    [Route("admin")]
    [Route("admin/Orders")]
    public class OrdersController : Controller
    {

        private readonly GlassesShopContext _context;
        private IBestSellerRespository _bestSellerRespository;
        public OrdersController(GlassesShopContext context, IBestSellerRespository bestSellerRespository)
        {
            _context = context;
            _bestSellerRespository = bestSellerRespository;
        }

		[AuthenAdmin]
		[Route("")]
        [Route("Index")]
        public IActionResult Index(int? page, int? statusId, DateTime? startDate, DateTime? endDate)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IQueryable<Order> orders = _context.Orders.Include(o => o.Customer)
                                                      .Include(o => o.Status)
                                                      .OrderByDescending(o => o.OrderDate);

            if (statusId.HasValue)
            {
                orders = orders.Where(o => o.StatusId == statusId);

                if (!orders.Any())
                {
                    ViewBag.Message = "No orders found";
                }
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate);

                if (!orders.Any())
                {
                    ViewBag.Message = "No orders found";
                }
            }
            ViewBag.StatusId = statusId;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            var order = orders.ToPagedList(pageNumber, pageSize);

            return View(order);
        }

        [AuthenAdmin]

		[Route("Details")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .Include(o => o.Address) // Include order details
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            //moi order detail la 1 sanpham: quantity price
            var orderdetails = await _context.OrderDetails.Where(m => m.OrderId == id).ToListAsync();
            ViewData["orderDetail"] = orderdetails;

            //get stock: name color 
            List<Group4_GlassesShop.Models.DataModel.Stock> stok = new List<Group4_GlassesShop.Models.DataModel.Stock>(); ;
            foreach (var o in orderdetails)
            {
                var stock = await _context.Stocks.Include(s => s.Color).Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.StockId == o.StockId);
                stok.Add(stock);
            }

            ViewData["stock"] = stok;
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [AuthenAdmin]


        [Route("")]
        [Route("Confirm")]
        public IActionResult Confirm(int id, int? statusId)
        {
            var orderSelect = _context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (orderSelect != null && orderSelect.StatusId == 1)
            {
                orderSelect.StatusId = 2;
                _context.SaveChanges();
            }
            else if (orderSelect != null && orderSelect.StatusId == 2)
            {
                orderSelect.StatusId = 3;
                _context.SaveChanges();

                // Cập nhật trạng thái BestSeller sau khi order đã hoàn thành (StatusId = 3)
                _bestSellerRespository.UpdateBestSellers();
            }

            if (statusId.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }




        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
