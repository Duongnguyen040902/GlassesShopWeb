using FakeItEasy;
using FluentAssertions;
using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;
using Group4_GlassesShop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRopositoryBlog.Tests.RepositoryTest
{
    public class RepositoryBlogTest
    {
        private IBlogRepository _IblogRepository;
        private GlassesShopContext _context;
        private BlogRepository _blogRepository;

        public RepositoryBlogTest()
        {
            _IblogRepository = A.Fake<IBlogRepository>();
            _context = A.Fake<GlassesShopContext>() ?? new GlassesShopContext();
            _blogRepository = new BlogRepository(_context);
        }
        [Fact]
        public void GetAllBlogTest()
        {
            // Arrange
            var blogs = new List<Blog>
            {
                new Blog { BlogId = 1, Title = "Blog 1", Content = "Content 1" },
                new Blog { BlogId = 2, Title = "Blog 2", Content = "Content 2" },
                // Thêm các blog khác nếu cần
            };

            A.CallTo(() => _context.Blogs).Returns(blogs.AsQueryable());

            // Act
            var result = _blogRepository.GetAllBlog();

            // Assert
            result.Should().BeEquivalentTo(blogs.Select(b => new BlogModel
            {
                BlogId = b.BlogId,
                Title = b.Title,
                Content = b.Content,
                PuslishDate = b.PuslishDate,
                CategoryBlogId = b.CategoryBlogId,
                Image = b.Image,
                Status = b.Status
            }));
        }
    }
}
