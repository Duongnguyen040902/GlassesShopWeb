using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Mvc.Core;
using X.PagedList;
//using Group4_GlassesShop.Models.ManagerProduct;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Group4_GlassesShop.Models.ManagerProduct;
using Group4_GlassesShop.Repositories;
using System.Linq.Expressions;
using Group4_GlassesShop.Infrastructure;

namespace Group4_GlassesShop.Controllers.HomePageClient
{
    public class ShopController : Controller
    {

        private readonly GlassesShopContext context;

        private readonly IProductRepository _productRepository;

        public ShopController(IProductRepository productRepository, GlassesShopContext context)
        {
            this.context = context;
            _productRepository = productRepository;

        }

        // Filter paging
        public async Task<ActionResult> Shop(ProductFilter filter)
        {
            var WishList = HttpContext.Session.GetJson<List<int>>("WishList");
            if (WishList != null) ViewBag.WishList = WishList;

            if (!string.IsNullOrWhiteSpace(filter.Sort))
            {
                var sortItems = filter.Sort.Split("-");
                if (sortItems != null && sortItems.Count() == 2)
                {
                    filter.SortBy = sortItems[0];
                    filter.SortDir = sortItems[1];
                }
            }

            var listCat = context.Categories.ToList();
            var listBrand = context.Brands.ToList();
            var listColor = context.Colors.ToList();

            var productQuery = await _productRepository.GetAsync(
                    predicate: filter.GetExpressions(),
                    orderBy: filter.GetOrder(),
                    includes: new List<Expression<Func<Product, object>>>
                    {
                        p => p.Category,
                        p => p.Images,
                        p => p.Brand
                        

                    }
                );

            var pagedList = new PagedList<Product>(productQuery, filter.Page, filter.PageSize);

            var viewModel = new ListManager
            {
                Products = pagedList.ToList(),
                Categories = listCat,
                Brand = listBrand,
                Color = listColor,
                PageList = pagedList,
                Filter = filter
            };

            return View(viewModel);
        }





        public ActionResult GetProductByPid(int pid)
        {
            var ProductById = context.Products.FirstOrDefault(p => p.Pid == pid);
            Product product = context.Products.Find(pid);
            List<Product> productRelate = context.Products.Include("Images").Where(x => x.CategoryId == product.CategoryId && x.Pid != product.Pid).Take(3).ToList();
            List<Feedback> feedbacks = context.Feedbacks.Include("Customer").Where(x => x.ProductId == pid)
            .Where(x => x.IsApproved == true)
            .ToList();
            ViewBag.ProductRelate = productRelate;
            ViewBag.Feedbacks = feedbacks;

            return View(ProductById);
        }
        public ActionResult AddWishList(int pid)
        {
            var WishList = HttpContext.Session.GetJson<List<int>>("WishList");
            if (WishList == null) WishList = new List<int>();
            var p = context.Products.SingleOrDefault(p => p.Pid == pid);
            if (p != null)
            {
                WishList.Add(p.Pid);

                HttpContext.Session.SetJon("WishList", WishList);
            }


            return RedirectToAction("Shop");
        }

        public ActionResult DeleteWishList(int id)
        {
            var WishList = HttpContext.Session.GetJson<List<int>>("WishList");
            if (WishList != null)
            {
                WishList.Remove(id);
            }
            HttpContext.Session.SetJon("WishList", WishList);


            return RedirectToAction("Shop");
        }


    }
}