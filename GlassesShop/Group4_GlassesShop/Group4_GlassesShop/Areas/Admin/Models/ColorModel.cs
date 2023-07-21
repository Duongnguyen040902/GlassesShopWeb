using System.ComponentModel.DataAnnotations;

namespace Group4_GlassesShop.Areas.Admin.Models
{
    public class ColorModel
    {
        public int ColorId { get; set; }
        
        [StringLength(5, MinimumLength = 5, ErrorMessage = "ColorName must be at least 5 characters.")]
        public string ColorName { get; set; } = null!;
    }
}
