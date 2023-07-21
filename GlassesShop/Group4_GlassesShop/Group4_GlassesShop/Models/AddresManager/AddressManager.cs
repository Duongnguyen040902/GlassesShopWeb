using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Models.AddresManager
{
    public class AddressManager
    {
        public int AddressID { get; set; }
        public string AddrDetail { get; set; }
        public Province Province { get; set; }
        public District District { get; set; }
        public Ward Ward { get; set; }
        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
        public List<Ward> Wards { get; set; }

        public string SelectedProvinceId { get; set; }
        public string SelectedDistrictId { get; set; }
        public string SelectedWardId { get; set; }

        public Customer Customer { get; set; }
        public Account Account { get; set; }
    }
}
