using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Models.Interface
{
    public interface IBlogRepository
    {
        List<BlogModel> GetAllBlog();
        BlogModel GetBlogById(int id);
        BlogModel Add(BlogModel blog);

        CategoryBlogModel AddCatgoryBlog(CategoryBlogModel category);
        int FindMaxCatBlog();
        void Update(int id, BlogModel blog);
        void Delete(int id);

        List<BlogModel> Search(string keySearch);


    }
}
