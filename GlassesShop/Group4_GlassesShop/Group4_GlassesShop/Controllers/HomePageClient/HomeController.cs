//using Group4_GlassesShop.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Group4_GlassesShop.Controllers.HomePageClient
{
    public class HomeController : Controller
    {
        Models.DataModel.GlassesShopContext Context = new Models.DataModel.GlassesShopContext();
        //private readonly IProduct _product;

        //public HomeController(IProduct product)
        //{
        //    _product = product;
        //}

        /// <HomePage>
        /// 
        /// Code Here
        /// 
        /// <HomePage>

        public ActionResult Index()
        {
            var bestSellers = Context.Products
                .Where(p => p.BestSeller)
                .ToList();

            return View(bestSellers);
        }


        /// <Details>
        /// 
        /// Code Here
        /// <Details>

        public ActionResult ProductDetails()
        {
            return View();
        }
    }
}
