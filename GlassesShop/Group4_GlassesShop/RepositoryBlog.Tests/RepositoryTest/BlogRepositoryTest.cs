using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;
using Group4_GlassesShop.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryBlog.Tests.RepositoryTest
{

    public class BlogRepositoryTest
    {
        private async Task<GlassesShopContext> GetContext()
        {
            var options = new DbContextOptionsBuilder<GlassesShopContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new GlassesShopContext(options);
            context.Database.EnsureCreated();
            if(await context.Blogs.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    context.Blogs.Add(new Blog
                    {
                        Title = "Test",
                        Content = "Test",
                        PuslishDate = DateTime.Now,
                        Image = "Okay",
                        CategoryBlogId = 2,
                        Status = true
                    });
                    await context.SaveChangesAsync();
                }
            }
            return context;
        }

        [Fact]
        public async void RepositoryBlog_Add_Returns()
        {
            //Arrange
            var blog = new BlogModel()
            {
                Title = "Test",
                Content = "Test",
                PuslishDate = DateTime.Now,
                Image = "Okay",
                CategoryBlogId = 2,
                Status = true
            };
            var context = await GetContext();
            var blogRepository = new BlogRepository(context);

            //Act
            var result = blogRepository.Add(blog);

            //assert
            Assert.NotNull(result);
            Assert.Equal(blog.Title, result.Title);
            Assert.Equal(blog.Content, result.Content);
            Assert.Equal(blog.PuslishDate, result.PuslishDate);
            Assert.Equal(blog.Image, result.Image);
            Assert.Equal(blog.CategoryBlogId, result.CategoryBlogId);
            Assert.Equal(blog.Status, result.Status);
            Assert.Equal(0, result.Viewcount);
        }
        [Fact]
        public async void RepositoryBlog_Add_Returns_InvalidBlogAsync()
        {
            // Arrange
            var blog = new BlogModel()
            {
                // Đặt một thuộc tính không hợp lệ, ví dụ: Title không được rỗng
                Title = "Test",
                Content = "",
                PuslishDate = DateTime.Now,
                Image = "",
                CategoryBlogId = 2,
                Status = true
            };
            var context = await GetContext();
            var blogRepository = new BlogRepository(context);

            // Act
            var result = blogRepository.Add(blog);
            // Assert
            Assert.True(result == null);
        }

    }
}
