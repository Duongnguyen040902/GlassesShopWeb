using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Stock
    {
        public int StockId { get; set; }
        public int ColorId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Stock()
        {
            
        }
        public Stock(int colorId, int productId, int quantity)
        {
            ColorId = colorId;
            ProductId = productId;
            Quantity = quantity;
        }

        public virtual Color Color { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
