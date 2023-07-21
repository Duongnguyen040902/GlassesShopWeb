using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int Bid { get; set; }
        public string Bname { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
