namespace Group4_GlassesShop.Models.DataModel
{
    public class OrderDetailModel
    {
        public int OrderId { get; set; }
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
