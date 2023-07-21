using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Areas.Admin.Models.Repository
{
    public interface IStockRepository
    {
        List<StockModel> getAllStock();
        List<StockModel> getAllStockById(int pid);
        StockModel getStock(int id);

        StockModel AddStock(StockModel stock,int ProductId,int quantity, int ColorId);

        void DeleteStock(int id);

        void UpdateStock(StockModel stock);
    }
}
