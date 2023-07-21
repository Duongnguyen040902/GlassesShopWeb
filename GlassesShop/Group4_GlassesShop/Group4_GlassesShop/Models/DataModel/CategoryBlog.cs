using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class CategoryBlog
    {
        public CategoryBlog()
        {
            Blogs = new HashSet<Blog>();
        }

        public int CategoryBlogId { get; set; }
        public string Cname { get; set; } = null!;

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
