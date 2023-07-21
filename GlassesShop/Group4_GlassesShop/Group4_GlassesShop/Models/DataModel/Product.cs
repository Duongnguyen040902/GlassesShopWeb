using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Product
    {
        public Product()
        {
            Feedbacks = new HashSet<Feedback>();
            Images = new HashSet<Image>();
            Stocks = new HashSet<Stock>();
        }

        public int Pid { get; set; }
        public string ProducctName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
        public int BrandId { get; set; }

        public decimal Price { get; set; }
        public int TypeId { get; set; }
        public int ShapeId { get; set; }
        public int FeatureId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool BestSeller { get; set; }

        public virtual Brand? Brand { get; set; } = null!;
        public virtual Category? Category { get; set; } = null!;
        public virtual Feature? Feature { get; set; } = null!;
        public virtual Shape? Shape { get; set; } = null!;
        public virtual Type? Type { get; set; } = null!;
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
