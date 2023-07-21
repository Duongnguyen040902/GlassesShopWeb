using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Models.MyOrderModel
{
    public class PaginatedOrderViewModel
    {
        public List<OrderViewModel> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? StatusId { get; set; }
        public string SearchKey { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

        public PaginatedOrderViewModel()
        {
            Orders = new List<OrderViewModel>();
        }
    }

}
