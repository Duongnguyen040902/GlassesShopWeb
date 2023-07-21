using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int Pid { get; set; }

        public virtual Product? PidNavigation { get; set; } = null!;
    }
}
