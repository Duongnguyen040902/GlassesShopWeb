using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class ChatDetail
    {
        public string Text { get; set; } = null!;
        public DateTime TimeChat { get; set; }
        public int ChatId { get; set; }
        public int Role { get; set; }
        public int ChatDeId { get; set; }

        public virtual Chat? Chat { get; set; } = null!;
    }
}
