using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group4_GlassesShop.Models.DataModel;
using System.Security;
using System.Linq.Dynamic.Core;
using Group4_GlassesShop.Areas.Admin.Models;
using Group4_GlassesShop.Areas.Admin.Models.Repository;
using Group4_GlassesShop.Areas.Admin.Models.ManagerStock;
using Group4_GlassesShop.Models.Authentication;

namespace Group4_GlassesShop.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/ManagerStock")]
    public class ManagerStockController : Controller
    {
        private readonly GlassesShopContext _context;
        private readonly IStockRepository _stockRepository;

        public ManagerStockController(IStockRepository stockRepository, GlassesShopContext context)
        {
            _stockRepository = stockRepository;
            _context = context;
        }

        [AuthenAdmin]
        [Route("")]
        [Route("ListStock")]
        public async Task<IActionResult> ListStock()
        {
            var listPro = await _context.Products
                        .Include(p => p.Images)
                        .Include(p => p.Brand)
                        .Include(p => p.Category)
                        .Include(p => p.Feature)
                        .Include(p => p.Shape)
                        .Include(p => p.Type)
                        .ToListAsync();


            listPro.Reverse();
            return View(listPro);
        }

        [AuthenAdmin]
        [Route("")]
        [Route("EditStock")]

        public IActionResult Edit(int id)
        {
            var getStockById = _stockRepository.getStock(id);
            if (getStockById != null)
            {
                return View(getStockById);
            }
            return NotFound();
        }

        [AuthenAdmin]
        [Route("")]
        [Route("AddStock")]
        [HttpGet]
        public IActionResult AddStock(int id)
        {
            var listStockByPid = _context.Products.SingleOrDefault(s => s.Pid == id);
            var getStockByPid = _stockRepository.getAllStockById(id);
            var listStockAll = new ListStock
            {
                GetProduct = listStockByPid,
                GetAllStockByPid = getStockByPid
            };
            return View(listStockAll);
        }

        [AuthenAdmin]
        [Route("")]
        [Route("AddStock")]
        [HttpPost]
        public IActionResult AddStock(int ColorId, int quantity,int ProductId, string NewColor)
        {
            if(ColorId == 0 && !string.IsNullOrEmpty(NewColor)) {
                var newColor = new Color { ColorName = NewColor };
                _context.Colors.Add(newColor);
                _context.SaveChanges();
                ColorId = newColor.ColorId;
            }
            var newStock = new StockModel(ColorId,quantity, ProductId);
            _stockRepository.AddStock(newStock, ProductId, quantity,ColorId);
            return RedirectToAction("ListStock");
        }



    }
}
