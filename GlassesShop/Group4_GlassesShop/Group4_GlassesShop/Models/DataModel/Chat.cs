using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Chat
    {
        public Chat()
        {
            ChatDetails = new HashSet<ChatDetail>();
        }

        public int ChatId { get; set; }
        public int AccId { get; set; }

        public virtual Account Acc { get; set; } = null!;
		[JsonIgnore]
		public virtual ICollection<ChatDetail> ChatDetails { get; set; }
    }
}
