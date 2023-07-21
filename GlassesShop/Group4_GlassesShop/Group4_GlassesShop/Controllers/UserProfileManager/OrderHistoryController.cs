using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
using System.Linq;
using static NuGet.Packaging.PackagingConstants;
using Group4_GlassesShop.Models.Interface;
using Group4_GlassesShop.Models.MyOrderModel;
using Group4_GlassesShop.Models.Authentication;

namespace Group4_GlassesShop.Controllers.UserProfileManager
{
    public class OrderHistoryController : Controller
    {
        private readonly IOrderRepository _orderManager;

        public OrderHistoryController(IOrderRepository orderManager)
        {
            _orderManager = orderManager;
        }
        GlassesShopContext db = new GlassesShopContext();

        /// <summary>
        /// Displays a list of orders associated with the current customer.
        /// </summary>
        /// <param name="searchKey">The search keyword to filter orders by product name.</param>
        /// <param name="statusId">The status ID to filter orders by status.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="fromDate">The start date to filter orders by order date.</param>
        /// <param name="toDate">The end date to filter orders by order date.</param>
        /// <returns>The view containing the list of orders with pagination.</returns>
        [Authentication]
        public async Task<IActionResult> MyOrderList(string searchKey = null, int? statusId = null, int? page = null, DateTime? fromDate = null, DateTime? toDate = null)
        {

            string userJson = HttpContext.Session.GetString("User");
            if (userJson == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                var isExitsUser = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(userJson);
                var customer = db.Customers.FirstOrDefault(c => c.AccountId == isExitsUser.AccountId);

                if (customer != null)
                {
                    List<OrderViewModel> orderViewModels = new List<OrderViewModel>();

                    // Get all orders associated with the current customer and order them by the order date in descending order
                    var allOrders = _orderManager.GetOrdersByCustomerId(customer.CustomerId)
                                                 .OrderByDescending(o => o.OrderDate);

                    IEnumerable<Order> filteredOrders = allOrders;

                    // Apply search filter based on product name
                    if (!string.IsNullOrEmpty(searchKey))
                    {
                        filteredOrders = _orderManager.GetOrderByProdcutNameAndCustomerId(searchKey, customer.CustomerId)
                                                      .OrderByDescending(o => o.OrderDate);



                        if (!filteredOrders.Any())
                        {
                            ViewBag.Message = "No search results.";
                        }
                    }
                    // Apply status filter
                    if (statusId.HasValue)
                    {
                        filteredOrders = filteredOrders.Where(o => o.StatusId == statusId)
                                                       .OrderByDescending(o => o.OrderDate);


                        if (!filteredOrders.Any())
                        {
                            ViewBag.Message = "No orders found";
                        }
                    }
                    // Apply date filter
                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        filteredOrders = filteredOrders.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                                                       .OrderByDescending(o => o.OrderDate);

                        if (!filteredOrders.Any())
                        {
                            ViewBag.Message = "No orders found";
                        }
                    }

                    ViewBag.SearchKey = searchKey;
                    ViewBag.StatusId = statusId;
                    ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd"); ;
                    ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd"); ;

                    int pageSize = 5;
                    int pageNumber = page ?? 1;

                    // Get the paged orders based on the filtered orders
                    var pagedOrders = await filteredOrders.OrderByDescending(o => o.OrderDate)
                                                 .ToPagedListAsync(pageNumber, pageSize);

                    // Iterate through each order and create a view model containing the order details, status, and product information
                    foreach (var order in pagedOrders)
                    {

                        var orderDetails = db.OrderDetails.Where(od => od.OrderId == order.OrderId).ToList();
                        var getStatus = db.Statuses.FirstOrDefault(s => s.StatusId == order.StatusId);

                        // Create a list of OrderDetailViewModel to store the order details of the current order
                        var orderDetailViewModels = orderDetails.Select(od =>
                        {
                            var stock = db.Stocks.FirstOrDefault(s => s.StockId == od.StockId);
                            var product = db.Products.FirstOrDefault(p => p.Pid == stock.ProductId);
                            var color = db.Colors.FirstOrDefault(c => c.ColorId == stock.ColorId);
                            var image = db.Images.FirstOrDefault(i => i.Pid == product.Pid);

                            return new OrderDetailViewModel
                            {
                                ProductId = product.Pid,
                                ProductName = product.ProducctName,
                                Image_Url = image.ImageUrl,
                                ColorName = color.ColorName,
                                Quantity = od.Quantity,
                                ProductPrice = product.Price
                            };
                        }).ToList();

                        // Create an OrderViewModel containing the order information, status, and order details
                        var orderViewModel = new OrderViewModel
                        {
                            OrderId = order.OrderId,
                            OrderDate = order.OrderDate,
                            OrderDetails = orderDetailViewModels,
                            status = getStatus,
                            TotalPrice = order.TotalPrice
                        };

                        orderViewModels.Add(orderViewModel);
                    }

                    // Create a PaginatedOrderViewModel containing the paged order view models and pagination information
                    var paginatedOrderViewModel = new PaginatedOrderViewModel
                    {
                        Orders = orderViewModels,
                        CurrentPage = pagedOrders.PageNumber,
                        TotalPages = pagedOrders.PageCount,
                    };

                    return View(paginatedOrderViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
        }

        /// <summary>
        /// Displays the details of an order based on the provided orderId.
        /// </summary>
        /// <param name="orderId">The ID of the order to display.</param>
        /// <returns>The view containing the order details.</returns>
        [Authentication]
        public IActionResult OrderDetail(int orderId)
        {
            var order = db.Orders.FirstOrDefault(o => o.OrderId == orderId);

            // If an order is found based on orderId, continue processing and preparing order information
            if (order != null)
            {
                var orderDetails = db.OrderDetails.Where(od => od.OrderId == orderId).ToList();
                List<OrderViewModel> orderViewModels = new List<OrderViewModel>();
                var getStatus = db.Statuses.FirstOrDefault(s => s.StatusId == order.StatusId);
                var customer = db.Customers.FirstOrDefault(cu => cu.CustomerId == order.CustomerId);
                var address = db.Addresses.FirstOrDefault(a => a.AddressId == order.AddressId);
                var province = db.Provinces.FirstOrDefault(p => p.PCode == address.ProvincesCode);
                var district = db.Districts.FirstOrDefault(d => d.DCode == address.DistrictsCode);
                var ward = db.Wards.FirstOrDefault(w => w.WCode == address.WardsCode);
                var orderDetailViewModels = orderDetails.Select(od =>
                {
                    // Fetch the stock, product, color, image, brand, type, shape, and category information for each order detail
                    var stock = db.Stocks.FirstOrDefault(s => s.StockId == od.StockId);
                    var product = db.Products.FirstOrDefault(p => p.Pid == stock.ProductId);
                    var color = db.Colors.FirstOrDefault(c => c.ColorId == stock.ColorId);
                    var image = db.Images.FirstOrDefault(i => i.Pid == product.Pid);
                    var brand = db.Brands.FirstOrDefault(b => b.Bid == product.BrandId);
                    var type = db.Types.FirstOrDefault(t => t.TypeId == product.TypeId);
                    var shape = db.Shapes.FirstOrDefault(sh => sh.ShapeId == product.ShapeId);
                    var feedback = db.Feedbacks.FirstOrDefault(f => f.ProductId == product.Pid);
                    var category = db.Categories.FirstOrDefault(ct => ct.CategoryId == product.CategoryId);
                    var status = db.Statuses.FirstOrDefault(s => s.StatusId == order.StatusId);

                    // Create an instance of OrderDetailViewModel with the required properties
                    return new OrderDetailViewModel
                    {

                        ProductId = product.Pid,
                        ProductName = product.ProducctName,
                        Image_Url = image.ImageUrl,
                        ColorName = color.ColorName,
                        Brand = brand.Bname,
                        BrandId = brand.Bid,
                        Type = type.TypeName,
                        Shape = shape.ShapeName,
                        feedback = feedback,
                        Category = category.Cname,
                        Quantity = od.Quantity,
                        ProductPrice = product.Price


                    };
                }).ToList();
                // Create an instance of OrderViewModel containing the order information, address information, status, and order details
                var orderViewModel = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    OrderDetails = orderDetailViewModels,
                    address = order.Address,
                    status = getStatus,
                    CustomerName = customer.Name,
                    CustomerPhone = customer.PhoneNumber,
                    ProvinceName = province.ProvinName,
                    DistrictName = district.DistrictName,
                    WardName = ward.WardsName,
                    AddDetail = address.AddDetails,
                    TotalPrice = order.TotalPrice
                };
                orderViewModels.Add(orderViewModel);
                return View("OrderDetail", orderViewModels);
            }
            else
            {
                return RedirectToAction("MyOrderList");
            }
        }

        /// <summary>
        /// POST
        /// Cancels an order based on the provided orderId.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        /// <returns>A JsonResult containing the result of the cancellation.</returns>
        [Authentication]
        [HttpPost]
        public JsonResult CancelOrder(int orderId)
        {
            try
            {

                var order = db.Orders.FirstOrDefault(o => o.OrderId == orderId);

                if (order != null)
                {
                    // If the order status is "Pending," change the order status to "Canceled" (StatusId = 4)
                    if (order.StatusId == 1)  
                    {
                        order.StatusId = 4; 
                        db.SaveChanges();
                        var getOrderDetail = db.OrderDetails.FirstOrDefault(o => o.OrderId == order.OrderId);
                        var reStock = db.Stocks.FirstOrDefault(s => s.StockId == getOrderDetail.StockId);
                        var reStockList = new List<Stock>();

                        // Add the quantity of the canceled order back to the stock quantity
                        if (reStock != null)
                        {
                            reStock.Quantity += getOrderDetail.Quantity;
                            reStockList.Add(reStock);
                            db.Stocks.Update(reStock);
                            db.SaveChanges();
                        }
                        // Return a JSON result indicating successful cancellation
                        return Json(new { success = true, message = "Order canceled successfully.." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Orders cannot be canceled." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Order not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error! An error occurred. Please try again later." });
            }
        }
    }

}