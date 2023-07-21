using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool? Role { get; set; }
        public bool Active { get; set; }

        public virtual Chat Chat { get; set; } = null!;
        public Account()
        {

        }

        public Account(string email, string password, bool? role, bool active)
        {
            Email = email;
            Password = password;
            Role = role;
            Active = active;
        }

        public virtual Customer Customer { get; set; } = null!;
    }
}
