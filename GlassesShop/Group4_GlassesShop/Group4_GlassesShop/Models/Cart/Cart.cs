using Group4_GlassesShop.Models.DataModel;
using System.Drawing;

namespace Group4_GlassesShop.Models.Cart
{
    public class Cart
    {
        public List<Item> _items { get; set; } = new List<Item>();       
        public void AddItem(Product product, int quantity, int colorId)
        {
            Item? existingItem = _items.FirstOrDefault(s => s.product.Pid == product.Pid && s.stock.ColorId == colorId);
            if (existingItem == null)
            {
                Stock stock = GetStock(product.Pid, colorId);
                if (stock != null)
                {
                    existingItem = new Item
                    {
                        product = product,
                        quantity = quantity,
                        stock = stock
                    };
                    _items.Add(existingItem);
                }
            }
            else
            {
                existingItem.quantity += quantity;
            }
        }

        private Stock GetStock(int productId, int colorId)
        {
            GlassesShopContext context = new GlassesShopContext();
            return context.Stocks.FirstOrDefault(s => s.ProductId == productId && s.ColorId == colorId);
        }
        public void ChangeQuantity(Product product, int quantity,int colorID)
        {
            Item? AddItem = _items.Where(s => s.product.Pid == product.Pid && s.stock.ColorId == colorID).FirstOrDefault();
            AddItem.quantity = quantity;
            if (quantity == 0)
            {
                RemoveItem(product, colorID);   
            }
        }
        public void RemoveItem(Product product , int colorID)
        {
            _items.RemoveAll(s => s.product.Pid == product.Pid && s.stock.ColorId == colorID);
        }

        public decimal ComputeTotalValue() => (decimal)_items.Sum(i => i.quantity * (i.product.Price));

        public void Clear()=> _items.Clear();
    }

    public class Item
    {
        public Product product { get; set; } = new Product();
        public Stock stock { get; set; } = new Stock();
        public int quantity { get; set; }
    }
}
