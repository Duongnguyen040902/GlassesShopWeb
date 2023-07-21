using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace Group4_GlassesShop.Controllers.BlogClient
{
    public class BlogDetailsController : Controller
    {
        GlassesShopContext context = new GlassesShopContext();
		#region BlogDetails
		public IActionResult BlogDetails(int blogId)
		{
			var BDetails = context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
			return View(BDetails);
		}
		#endregion

	}
}
