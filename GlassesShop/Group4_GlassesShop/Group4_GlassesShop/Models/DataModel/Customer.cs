using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Customer
    {
        public Customer()
        {
            Addresses = new HashSet<Address>();
            Feedbacks = new HashSet<Feedback>();
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int AccountId { get; set; }

        public Customer(string name, string phoneNumber, string? avatarUrl)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            AvatarUrl = avatarUrl;
        }
        public Customer(string name, int accountId, string phoneNumber, string? avatarUrl)
        {
            Name = name;
            AccountId = accountId;
            PhoneNumber = phoneNumber;
            AvatarUrl = avatarUrl;
        }



        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
