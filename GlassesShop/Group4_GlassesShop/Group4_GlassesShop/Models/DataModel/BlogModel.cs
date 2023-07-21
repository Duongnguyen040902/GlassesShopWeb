using System.ComponentModel.DataAnnotations;

namespace Group4_GlassesShop.Models.DataModel
{
    public class BlogModel
    {
        public int BlogId { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(10, ErrorMessage = "Title must be at least 10 words.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [MinLength(200, ErrorMessage = "Content must contain at least 200 words.")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Publish Date is required.")]
        [DataType(DataType.Date)]
        public DateTime PuslishDate { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryBlogId { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string Image { get; set; }
        public bool Status { get; set; }
        public int? Viewcount { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string ? KeySearch { get; set; }
        public IEnumerable<BlogModel> Blogs { get; set; }

    }
}
