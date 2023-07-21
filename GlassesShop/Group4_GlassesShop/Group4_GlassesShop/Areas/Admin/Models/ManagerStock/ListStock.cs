using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Areas.Admin.Models.ManagerStock
{
    public class ListStock
    {
        public List<StockModel> GetAllStockByPid { get; set; }
        public Product GetProduct { get; set; }

        public StockModel GetStock { get; set; }
    }
}
