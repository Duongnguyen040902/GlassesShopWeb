using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;
using Group4_GlassesShop.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using X.PagedList;

namespace Group4_GlassesShop.Areas.BlogManager.Controllers
{
    [Area("BlogManager")]
    [Route("BlogManager")]
    public class ManagerBlogController : Controller
    {
        private IBlogRepository _blogRepository;

        public ManagerBlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
		#region ManagerBlog
		[Route("")]
		[Route("Index")]
		[HttpGet]
		public IActionResult Index(int? page)
		{
			int pageSize = 10;
			int pageNumber = (page ?? 1);
			try
			{
				if (TempData["SuccessMessage"] != null)
				{
					ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
				}
				if (TempData["CreateSuccess"] != null)
				{
					ViewBag.CreateSuccess = TempData["CreateSuccess"].ToString();
				}
				var pageBlogs = _blogRepository.GetAllBlog().ToPagedList(pageNumber, pageSize);
				int totalItem = pageBlogs.TotalItemCount;
				int totalPages = (int)Math.Ceiling((double)totalItem / pageSize);
				var model = new BlogModel
				{
					Blogs = pageBlogs,
					CurrentPage = pageNumber,
					TotalPages = totalPages
				};
				return View(model);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[Route("CreateBlog")]
		[HttpGet]
		public IActionResult CreateBlog()
		{
			return View(new BlogModel());
		}

		[Route("CreateBlog")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateBlog(BlogModel model, string OtherCategory)
		{
			var validator = new BlogValidation();
			var validationResult = validator.Validate(model);
			if (validationResult.IsValid)
			{
				if (model.CategoryBlogId == -1)
				{
					int maxCategoryBlogId = _blogRepository.FindMaxCatBlog();
					int otherOptionValue = maxCategoryBlogId + 1;
					string _otherCat = OtherCategory;
					var NewCat = new CategoryBlogModel { CategoryBlogId = otherOptionValue, Cname = _otherCat };
					_blogRepository.AddCatgoryBlog(NewCat);
					model.CategoryBlogId = otherOptionValue;
					_blogRepository.Add(model);
					TempData["CreateSuccess"] = "Create Successfully!";
					return RedirectToAction("Index");
				}
				else
				{
					_blogRepository.Add(model);
                    TempData["CreateSuccess"] = "Create Successfully!";
                    return RedirectToAction("Index");
				}
			}
			foreach (var error in validationResult.Errors)
			{
				ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}
			return View(model);
		}

		[HttpGet("{BlogId}")]
		public IActionResult GetBlogById(int BlogId)
		{
			try
			{
				var checkExitsBlog = _blogRepository.GetBlogById(BlogId);
				if (checkExitsBlog != null)
				{
					return View(checkExitsBlog);
				}
				return NotFound();
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpGet]
		[Route("EditBlog")]
		public IActionResult EditBlog(int BlogId)
		{
			try
			{
				var BlogById = _blogRepository.GetBlogById(BlogId);
				if (BlogById != null)
				{
					return View(BlogById);
				}
				return NotFound();
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPost]
		[Route("EditBlog")]
		public IActionResult EditBlog(int BlogId, BlogModel model)
		{
			var validator = new BlogValidation();
			var validationResult = validator.Validate(model);

			if (validationResult.IsValid)
			{
				_blogRepository.Update(BlogId, model);
				TempData["SuccessMessage"] = "Edit Successfully!";
				return RedirectToAction("Index");
			}

			// Hiển thị các thông báo lỗi
			foreach (var error in validationResult.Errors)
			{
				ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}

			return View(model);
		}

		[HttpGet]
		[Route("DetailsBlog")]
		public IActionResult DetailsBlog(int BlogId)
		{
			var getBlogById = _blogRepository.GetBlogById(BlogId);
			if (getBlogById != null)
			{
				return View(getBlogById);
			}
			return NotFound();
		}

		[Route("DeleteBlog")]
		public IActionResult DeleteBlog(int blogId)
		{
			try
			{
				_blogRepository.Delete(blogId);
				TempData["DeleteSuccessMessage"] = "Delete successful!";
			}
			catch (Exception ex)
			{
				TempData["DeleteErrorMessage"] = "Delete failed: " + ex.Message;
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[Route("SearchBlog")]
		public IActionResult SearchBlog(string keySearch, int? page)
		{
			int pageSize = 10;
			int pageNumber = page ?? 1;
			var listBlogSearch = _blogRepository.Search(keySearch).ToPagedList(pageNumber, pageSize);
			int totalItem = listBlogSearch.TotalItemCount;
			int totalPages = (int)Math.Ceiling((double)totalItem / pageSize);
			var model = new BlogModel
			{
				KeySearch = keySearch,
				Blogs = listBlogSearch,
				CurrentPage = pageNumber,
				TotalPages = totalPages
			};
			return PartialView("_PartialSearchBlog", model);
		}
		#endregion

	}
}
