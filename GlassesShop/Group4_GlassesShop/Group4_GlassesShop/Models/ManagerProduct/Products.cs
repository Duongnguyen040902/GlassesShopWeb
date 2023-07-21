//using Group4_GlassesShop.Models.DataModel;
//using Group4_GlassesShop.Models.Interface;
//using Inventory.Domain;
//using Microsoft.EntityFrameworkCore;

//namespace Group4_GlassesShop.Models.ManagerProduct
//{
//    public class Products : IProduct
//    {
//        private readonly InventoryContext _context;
//        private string _errors = "";
//        public Products(InventoryContext context)
//        {
//            _context = context;
//        }
//        public bool Create(Product product)
//        {
//            try
//            {
//                _context.Products.Add(product);
//                _context.SaveChanges();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _errors = "Sql Exception Ocured, Error Info: " + ex.Message;
//                return false;
//            }

//        }

//        public bool Delete(Product product)
//        {
//            try
//            {
//                _context.Products.Attach(product);
//                _context.Entry(product).State = EntityState.Deleted;
//                _context.SaveChanges();
//                return true;
//            }
//            catch (Exception ex)
//            {

//                _errors = "Sql Exception Ocured, Error Info: " + ex.Message;
//                return false;
//            }
//        }

//        public string GetError()
//        {
//            return _errors; 
//        }

//        public List<Product> GetListProduct()
//        {
//            try
//            {
//                return _context.Products.ToList();
//            }
//            catch (Exception ex)
//            {
//                _errors = "Sql Exception Ocured, Error Info: " + ex.Message;
//                return null;
//            }
//        }



//        public Product GetProductById(int id)
//        {
//            Product productsById = _context.Products.Where(p => p.Pid == id).FirstOrDefault();
//            return productsById;
//        }

//        public List<Product> GetProductByImage()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Update(Product product)
//        {
//            try
//            {
//                _context.Products.Attach(product);
//                //Modified muon chinh sua dung modified
//                _context.Entry(product).State = EntityState.Modified;
//                _context.SaveChanges();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _errors = "Sql Exception Ocured, Error Info: " + ex.Message;
//                return false;
//            }
//        }
//    }
//}
