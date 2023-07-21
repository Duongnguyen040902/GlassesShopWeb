using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string Cname { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
