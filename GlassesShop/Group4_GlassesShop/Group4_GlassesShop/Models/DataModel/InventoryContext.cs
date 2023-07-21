using Microsoft.EntityFrameworkCore;

namespace Group4_GlassesShop.Models.DataModel
{
    public class InventoryContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
    {
    }

    public Product GetProductById(int id)
    {
        return Products.Find(id);
    }

    public List<Product> GetListProduct()
    {
        return Products.ToList();
    }

    public bool Create(Product product)
    {
        Products.Add(product);
        SaveChanges();
        return true;
    }

    public bool Delete(Product product)
    {
        Products.Remove(product);
        SaveChanges();
        return true;
    }

    public bool Update(Product product)
    {
        Products.Update(product);
        SaveChanges();
        return true;
    }
    }
}
