using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Models.MyOrderModel
{
    public class OrderViewModel
    {
        public List<OrderDetailViewModel> OrderDetails { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductId { get; set; }
        public Status status { get; set; }
        public int OrderId { set; get; }
        public DateTime OrderDate { get; set; }
        public Address address { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { set; get; }
        public string AddDetail { set; get; }
        public decimal ProductPrice { get; set; }

        public OrderViewModel()
        {
            OrderDetails = new List<OrderDetailViewModel>();
        }
        public int SelectedStatusId { get; set; }
    }

    public class OrderDetailViewModel
    {


        public int ProductId { set; get; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string Brand { set; get; }
        public int BrandId { set; get; }
        public string Type { set; get; }
        public string Shape { set; get; }
        public string Category { set; get; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Status status { get; set; }
        public Address address { get; set; }
        public string Image_Url { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ProductPrice { get; set; }

        public Feedback? feedback { get; set; }


        public OrderDetailViewModel()
        {

        }




    }
}
