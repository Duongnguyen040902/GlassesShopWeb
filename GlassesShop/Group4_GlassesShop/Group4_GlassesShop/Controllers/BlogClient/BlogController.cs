using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Group4_GlassesShop.Controllers.BlogClient
{
    public class BlogController : Controller
    {
        private GlassesShopContext _context;

        public BlogController(GlassesShopContext context)
        {
            _context = context;
        }
		#region Blog
		/// <summary>
		/// GET: Blog
		/// Returns the blog view with a paginated list of blogs.
		/// </summary>
		/// <param name="page">The page number for pagination.</param>
		/// <returns>Returns the blog view with a paginated list of blogs.</returns>
		[HttpGet]
		public IActionResult Blog(int? page)
		{
			int pageSize = 3;
			int pageNumber = (page ?? 1);

			var blogs = _context.Blogs.ToPagedList(pageNumber, pageSize);

			return View(blogs);
		}

		/// <summary>
		/// POST: Blog
		/// Handles the blog search form submission and returns the blog view with search results.
		/// </summary>
		/// <param name="page">The page number for pagination.</param>
		/// <param name="searchKey">The search keyword entered by the user.</param>
		/// <returns>Returns the blog view with search results based on the search keyword.</returns>
		[HttpPost]
		public IActionResult Blog(int? page, string searchKey)
		{
			int pageSize = 3;
			int pageNumber = (page ?? 1);

			IQueryable<Blog> blogsQuery = _context.Blogs;

			if (!string.IsNullOrEmpty(searchKey))
			{
				blogsQuery = blogsQuery.Where(b => b.CategoryBlog.Cname.Contains(searchKey) || b.Title.Contains(searchKey));
			}
			else
			{
				blogsQuery = blogsQuery.Where(b => true);
			}

			var blogs = blogsQuery.OrderBy(b => b.PuslishDate).ToPagedList(pageNumber, pageSize);

			if (!blogs.Any() && !string.IsNullOrEmpty(searchKey))
			{
				ViewBag.Message = "Không tìm thấy kết quả phù hợp.";
			}

			return View(blogs);
		}

		/// <summary>
		/// Returns the blogs filtered by category as a partial view.
		/// </summary>
		/// <param name="CategoryBlogId">The ID of the category to filter blogs.</param>
		/// <returns>Returns the blogs filtered by category as a partial view.</returns>
		public IActionResult GetBlogsByCategory(int CategoryBlogId)
		{
			var blogs = _context.Blogs.Where(b => b.CategoryBlogId == CategoryBlogId).ToList();
			return PartialView("_BlogList", blogs);
		}
		#endregion

	}
}
