using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Feature
    {
        public Feature()
        {
            Products = new HashSet<Product>();
        }

        public int FeatureId { get; set; }
        public string FeatureName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
