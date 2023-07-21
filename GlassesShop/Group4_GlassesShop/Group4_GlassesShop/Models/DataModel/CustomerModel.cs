namespace Group4_GlassesShop.Models.DataModel
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int AccountId { get; set; }
    }
}
