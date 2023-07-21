using Group4_GlassesShop.Infrastructure;
using Group4_GlassesShop.Models.Authentication;
using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Security.Cryptography;

namespace Group4_GlassesShop.Controllers.WishList
{
    [Authentication]
    public class WishListController : Controller
    {
        GlassesShopContext _context;
        IProductRepository _productRepository;
        
        public WishListController(GlassesShopContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }


        // GET: WishListController
        [Authentication]
        public async Task<ActionResult> Index()
        {
            try
            {
                var WishList = HttpContext.Session.GetJson<List<int>>("WishList");
                ViewBag.WishList = WishList;

                List<Product> WishListProduct = new List<Product>();
                if (WishList != null)
                {
                    foreach (int id in WishList)
                    {
                        var p = _context.Products.Include(p => p.Category).Include(p => p.Brand).SingleOrDefault(p => p.Pid == id);
                        p.Images = _context.Images.Where(i => i.Pid == p.Pid).ToList();
                        if (p != null) WishListProduct.Add(p);
                    }
                }
                
                return View(WishListProduct);
            }
            catch
            {

            }
            return RedirectToAction("Index","Home");
        }

        // GET: WishListController/Details/5
        [Authentication]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WishListController/Create
        [Authentication]
        public ActionResult Create(int pid)
        {
            var WishList = HttpContext.Session.GetJson<List<int>>("WishList");
            var p = _context.Products.Include(p => p.Images).Include(p => p.Category).Include(p => p.Brand).SingleOrDefault(p => p.Pid == pid);
            if (p != null)
            {
                WishList.Add(pid);

                HttpContext.Session.SetJon("WishList", WishList);
            }
               

            return RedirectToAction("Index");
        }

        // POST: WishListController/Create
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WishListController/Edit/5
        [Authentication]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WishListController/Edit/5
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WishListController/Delete/5
        [Authentication]
        public ActionResult Delete(int id)
        {
            var WishList = HttpContext.Session.GetJson<List<int>>("WishList");
            if (WishList != null)
            {
                WishList.Remove(id);
            }
            HttpContext.Session.SetJon("WishList", WishList);


            return RedirectToAction("Index");
        }

        // POST: WishListController/Delete/5
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
