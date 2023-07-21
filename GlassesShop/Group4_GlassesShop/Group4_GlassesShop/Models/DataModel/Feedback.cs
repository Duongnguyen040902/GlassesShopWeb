using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Feedback
    {
        public int ProductId { get; set; }
        public string Content { get; set; } = null!;
        public int CustomerId { get; set; }
        public int FeedbackId { get; set; }

        public int Rating { get; set; }

        public bool IsApproved { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
