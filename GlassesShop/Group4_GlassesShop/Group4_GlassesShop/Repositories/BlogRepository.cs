using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.SqlServer;

namespace Group4_GlassesShop.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly GlassesShopContext _context;

        public BlogRepository(GlassesShopContext context)
        {
            _context = context;
        }
        public BlogModel Add(BlogModel blog)
        {
            //if (blog.Title.Equals("")
            //    || blog.Content.Equals("")
            //    || blog.PuslishDate.Equals("")
            //    || blog.CategoryBlogId == 0
            //    || blog.Image.Equals("")
            //    || blog.Status.Equals(""))
            //{
            //    return null;
            //}

            var _blogs = new Blog
            {
                Title = blog.Title,
                Content = blog.Content,
                PuslishDate = DateTime.Now,
                CategoryBlogId = blog.CategoryBlogId,
                Image = "/ImgBlog/" + blog.Image,
                Status = blog.Status,
                Viewcount = 0
            };
            _context.Blogs.Add(_blogs);
            _context.SaveChanges();
            return new BlogModel
            {
                Title = blog.Title,
                Content = blog.Content,
                PuslishDate = blog.PuslishDate,
                CategoryBlogId = blog.CategoryBlogId,
                Image = blog.Image,
                Status = blog.Status,
                Viewcount = 0
            };
        }


        public CategoryBlogModel AddCatgoryBlog(CategoryBlogModel category)
        {
            var _categoryBlogs = new CategoryBlog { Cname = category.Cname, };
            _context.CategoryBlogs.Add(_categoryBlogs);
            _context.SaveChanges();
            return new CategoryBlogModel { CategoryBlogId = category.CategoryBlogId, Cname = category.Cname, };
        }

        public void Delete(int id)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.BlogId == id);
            if (blog != null)
            {
                _context.Remove(blog);
                _context.SaveChanges();
            }
        }

        public int FindMaxCatBlog()
        {
            int max = _context.CategoryBlogs.Max(catBlog => catBlog.CategoryBlogId);
            return max;
        }

        public List<BlogModel> GetAllBlog()
        {
            var blogs = _context.Blogs
                .OrderByDescending(b => b.PuslishDate)
                .Select(b => new BlogModel
                {
                    BlogId = b.BlogId,
                    Title = b.Title,
                    Content = b.Content,
                    PuslishDate = b.PuslishDate,
                    CategoryBlogId = b.CategoryBlogId,
                    Image = b.Image,
                    Status = b.Status,
                })
                .ToList();

            return blogs;
        }

        public BlogModel GetBlogById(int id)
        {
            var blog = _context.Blogs.SingleOrDefault(b => b.BlogId == id);
            if (blog != null)
            {
                return new BlogModel
                {
                    BlogId = blog.BlogId,
                    Title = blog.Title,
                    Content = blog.Content,
                    PuslishDate = blog.PuslishDate,
                    CategoryBlogId = blog.CategoryBlogId,
                    Image = blog.Image,
                    Status = blog.Status,
                    Viewcount = blog.Viewcount
                };
            }
            return null;
        }

        public List<BlogModel> Search(string keySearch)
        {
            IQueryable<Blog> query = _context.Blogs;

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => EF.Functions.Like(b.Title, $"%{keySearch}%"));
            }

            var listBlogSearch = query.Select(b => new BlogModel
            {
                BlogId = b.BlogId,
                Title = b.Title,
                Content = b.Content,
                PuslishDate = b.PuslishDate,
                CategoryBlogId = b.CategoryBlogId,
                Image = b.Image
            })
            .ToList();

            return listBlogSearch;
        }


        public void Update(int id, BlogModel blog)
        {
            var _blog = _context.Blogs.SingleOrDefault(b => b.BlogId == id);
            if (_blog != null)
            {
                _blog.Title = blog.Title;
                _blog.Content = blog.Content;
                _blog.PuslishDate = DateTime.Now;
                _blog.CategoryBlogId = blog.CategoryBlogId;
                _blog.Image = "/ImgBlog/" + blog.Image;
                _blog.Status = blog.Status;
                _context.Update(_blog);
                _context.SaveChanges();
            }
        }
    }
}
