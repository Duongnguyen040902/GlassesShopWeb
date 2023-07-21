using DemoWebFirstMVCCore;
using Group4_GlassesShop.Infrastructure;
using Group4_GlassesShop.Models.AddresManager;
using Group4_GlassesShop.Models.Cart;
using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using System.Security.Principal;
using System.Text.Json.Serialization;
using System.Text.Json;
using Group4_GlassesShop.Models.Authentication;

namespace Group4_GlassesShop.Controllers.CartShopping
{
    public class CartController : Controller
    {
        public Cart? cart { get; set; }
        private readonly IEmailSender _emailSender;
        private readonly GlassesShopContext _context = new GlassesShopContext();
        public CartController(IEmailSender emailSender , GlassesShopContext context) 
        {
            _emailSender = emailSender;
            _context = context;
        }

        /// <Cart>
        /// 
        /// Code Here
        /// <Cart>

        public IActionResult AddToCart(int Pid,int colorID,int quantity)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.Pid == Pid);

            if (product == null)
            {
                return NotFound(); // Không tìm thấy sản phẩm
            }
           
                cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();          
                cart.AddItem(product, quantity, colorID);
                HttpContext.Session.SetJon("cart", cart);

            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            cart = HttpContext.Session.GetJson<Cart>("cart");
            int cartItemCount = cart != null ? cart._items.Count: 0;
            HttpContext.Session.SetInt32("CartItemCount", cartItemCount);
            if (cart != null)
            {
                cart = HttpContext.Session.GetJson<Cart>("cart");

                return View(cart);
            }

            Cart emptyCart = new Cart();

            return View(emptyCart);
        }

        public IActionResult RemoveCart(int Pid,int colorID)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.Pid == Pid);
            if (product != null)
            {
                cart = HttpContext.Session.GetJson<Cart>("cart");
                cart.RemoveItem(product, colorID);
                HttpContext.Session.SetJon("cart", cart);
            }
            return RedirectToAction("Cart");
        }

        public IActionResult ChangeQuantity(int Pid, int quantity, int colorID)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.Pid == Pid);
            if (product != null)
            {
                cart = HttpContext.Session.GetJson<Cart>("cart");
                cart.ChangeQuantity(product, quantity, colorID);
                HttpContext.Session.SetJon("cart", cart);
            }
            return RedirectToAction("Cart");
        }

        //
        [Authentication]
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetJson<Cart>("cart");
            HttpContext.Session.SetJon("cart", cart);
            //List<Cart> carts = new List<Cart>();
            string accountJson = HttpContext.Session.GetString("User");
            var account = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.AccountModel>(accountJson);
            var Customer = _context.Customers.FirstOrDefault(c => c.AccountId == account.AccountId);
            HttpContext.Session.SetJon("emailOrder", Customer);
            var getAllProvinces = _context.Provinces.ToList();
            var getAllDistricts = _context.Districts.ToList();
            var getAllWards = _context.Wards.ToList();
            var newAdress = new AddressManager
            {
                Provinces = getAllProvinces,
                Districts = getAllDistricts,
                Wards = getAllWards
            };
            var CheckoutVM = new CheckoutModel
            {
                Carts = cart,
                Customer = Customer,
                Address = newAdress,
            };
            return View(CheckoutVM);
        }

        [HttpPost]

        public async Task<IActionResult> CheckoutAsync(CheckoutModel model,string addressMain , string SelectedProvinceId , string SelectedDistrictId , string SelectedWardId , string addressDetails)
        {
            int addressID = 0;
            var cart = HttpContext.Session.GetJson<Cart>("cart");
            HttpContext.Session.SetJon("cart", cart);
            var isExitsAccount = HttpContext.Session.GetJson<Account>("User");
            var Customer = HttpContext.Session.GetJson<Customer>("emailOrder");
            if (!string.IsNullOrEmpty(addressMain) && addressMain == "other")
            {
                var provinceId = SelectedProvinceId;
                var districtId = SelectedDistrictId;
                var wardId = SelectedWardId;
                var addressDetail = addressDetails;
                var newAddress = new Address
                {
                    ProvincesCode = provinceId,
                    DistrictsCode = districtId,
                    WardsCode = wardId,
                    AddDetails = addressDetails,
                    CustomerId = Customer.CustomerId
                };
                _context.Addresses.Add(newAddress);
                _context.SaveChanges();
                addressID += newAddress.AddressId;
            }
            else
            {
                addressID += model.Order.AddressId = int.Parse(addressMain);
            }
            var order = model.Order;
            order.CustomerId = Customer.CustomerId;
            order.OrderDate = DateTime.Now;
            order.TotalPrice = cart.ComputeTotalValue();
            order.Status = order.Status;
            order.AddressId = addressID;
            _context.Add(order);
            _context.SaveChanges();
            int orderID = order.OrderId;
            HttpContext.Session.SetInt32("orderCus", orderID);
            var orderDetailList = new List<OrderDetail>();
            var stockProductList = new List<Stock>();
            foreach (var cartItem in cart._items)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    StockId = cartItem.stock.StockId,
                    Quantity = cartItem.quantity,
                    Price = cartItem.product.Price * cartItem.quantity
                };
                orderDetailList.Add(orderDetail);

                var stockProduct = _context.Stocks.FirstOrDefault(p => p.StockId == cartItem.stock.StockId);
                if (stockProduct != null)
                {
                    stockProduct.Quantity -= cartItem.quantity;
                    stockProductList.Add(stockProduct);
                }
            }

            _context.AddRange(orderDetailList);
            _context.UpdateRange(stockProductList);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Order Successfully";
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 1000 // Đặt độ sâu tối đa theo nhu cầu của bạn
            };
            HttpContext.Session.Remove("cart");
            HttpContext.Session.SetInt32("CartItemCount", 0);
            string customerJson = System.Text.Json.JsonSerializer.Serialize(order, options);
            //HttpContext.Session.SetJon("CusOrder", order);
            var emailCus = _context.Accounts.Where(a => a.Customer.CustomerId == Customer.CustomerId).FirstOrDefault();
            await _emailSender.SendEmailOrderAsync(emailCus.Email);
            return RedirectToAction("Index","Home");
        }

        public IActionResult GetDistricts(string provinceCode)
        {
            var districts = _context.Districts.Where(d => d.ProvinceCode == provinceCode).ToList();
            return PartialView("_Districts", districts);
        }
        public IActionResult GetWards(string districtId)
        {
            var wards = _context.Wards.Where(w => w.DistrictCode == districtId).ToList();
            return PartialView("_Ward", wards);
        }
    }
}
