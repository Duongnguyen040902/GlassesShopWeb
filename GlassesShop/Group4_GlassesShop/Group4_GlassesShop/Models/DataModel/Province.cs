using System;
using System.Collections.Generic;

namespace Group4_GlassesShop.Models.DataModel
{
    public partial class Province
    {
        public Province()
        {
            Addresses = new HashSet<Address>();
            Districts = new HashSet<District>();
        }

        public string PCode { get; set; } = null!;
        public string ProvinName { get; set; } = null!;
        public string? NameEn { get; set; }
        public string FullName { get; set; } = null!;
        public string? FullNameEn { get; set; }
        public string? CodeName { get; set; }
        public int? AdministrativeUnitId { get; set; }
        public int? AdministrativeRegionId { get; set; }

        public virtual AdministrativeRegion? AdministrativeRegion { get; set; }
        public virtual AdministrativeUnit? AdministrativeUnit { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
