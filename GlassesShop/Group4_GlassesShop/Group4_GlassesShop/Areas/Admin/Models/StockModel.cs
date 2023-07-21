namespace Group4_GlassesShop.Areas.Admin.Models
{
    public class StockModel
    {
        public int StockId { get; set; }
        public int ColorId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public StockModel()
        {
            
        }
        public StockModel(int colorId, int productId, int quantity)
        {
            ColorId = colorId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
