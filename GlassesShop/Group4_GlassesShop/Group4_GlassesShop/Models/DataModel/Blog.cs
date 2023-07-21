using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime PuslishDate { get; set; }
        public int CategoryBlogId { get; set; }
        public string Image { get; set; } = null!;
        public bool Status { get; set; }
        public int? Viewcount { get; set; }

        public virtual CategoryBlog CategoryBlog { get; set; } = null!;
    }
}
