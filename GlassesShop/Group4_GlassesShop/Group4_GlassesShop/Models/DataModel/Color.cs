using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Color
    {
        public Color()
        {
            Stocks = new HashSet<Stock>();
        }

        public int ColorId { get; set; }
        public string ColorName { get; set; } = null!;
        

        public virtual ICollection<Stock> Stocks { get; set; }

    }
}
