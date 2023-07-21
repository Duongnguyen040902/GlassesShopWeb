using System.ComponentModel.DataAnnotations;

namespace Group4_GlassesShop.Models.DataModel
{
    public class CategoryBlogModel
    {
        public int CategoryBlogId { get; set; }
        [Required]
        [MinLength(10)]
        public string Cname { get; set; }
    }
}
