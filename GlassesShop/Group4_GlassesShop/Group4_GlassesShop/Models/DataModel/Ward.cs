using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Ward
    {
        public Ward()
        {
            Addresses = new HashSet<Address>();
        }

        public string WCode { get; set; } = null!;
        public string WardsName { get; set; } = null!;
        public string? WNameEn { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public string? CodeName { get; set; }
        public string? DistrictCode { get; set; }
        public int? AdministrativeUnitId { get; set; }

        public virtual AdministrativeUnit? AdministrativeUnit { get; set; }
        public virtual District? DistrictCodeNavigation { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
