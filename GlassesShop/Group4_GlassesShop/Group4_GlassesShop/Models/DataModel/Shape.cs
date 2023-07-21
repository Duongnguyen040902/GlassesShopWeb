using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Shape
    {
        public Shape()
        {
            Products = new HashSet<Product>();
        }

        public int ShapeId { get; set; }
        public string ShapeName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
