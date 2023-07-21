using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Address
    {
        public Address()
        {
            Orders = new HashSet<Order>();
        }
        public Address(string wardsCode, string provincesCode, string districtsCode, int customerId, string? addDetails)
        {
            WardsCode = wardsCode;
            ProvincesCode = provincesCode;
            DistrictsCode = districtsCode;
            CustomerId = customerId;
            AddDetails = addDetails;
        }

        public int AddressId { get; set; }
        public string WardsCode { get; set; } = null!;
        public string ProvincesCode { get; set; } = null!;
        public string DistrictsCode { get; set; } = null!;
        public int CustomerId { get; set; }
        public string? AddDetails { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual District DistrictsCodeNavigation { get; set; } = null!;
        public virtual Province ProvincesCodeNavigation { get; set; } = null!;
        public virtual Ward WardsCodeNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }

    }
}
