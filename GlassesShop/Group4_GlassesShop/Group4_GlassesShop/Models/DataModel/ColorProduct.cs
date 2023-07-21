using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class ColorProduct
    {
        public ColorProduct()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ColorId { get; set; }
        public int Pid { get; set; }
        public int Quantity { get; set; }
        public string ColorName { get; set; } = null!;

        public virtual Product? PidNavigation { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
