using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group4_GlassesShop.Models.DataModel;
using System.Security;
using System.Linq.Dynamic.Core;
using NuGet.Configuration;
using System.Drawing;
using System.Collections;
using X.PagedList;
using Group4_GlassesShop.Models.Authentication;

namespace Group4_GlassesShop.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/Products")]
    public class ProductsController : Controller
    {

        private readonly GlassesShopContext _context;

        public ProductsController(GlassesShopContext context)
        {
            _context = context;
        }




        //get list product
        [Route("")]
        [Route("Index")]
        [AuthenAdmin]

        // GET: Products
        public IActionResult Index(int? page)
        {
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            var lispro = _context.Products
                        .Include(p => p.Brand)
                        .Include(p => p.Category)
                        .Include(p => p.Feature)
                        .Include(p => p.Shape)
                        .Include(p => p.Type)
                        .ToList();
            lispro.Reverse();

            var reversedLispro = lispro.ToPagedList(pageNumber, pageSize);

            return View(reversedLispro);
        }









        //search product
		[AuthenAdmin]
		[Route("")]
        [Route("admindashboard/Search")]
        [Route("Search")]
        public IActionResult Search(string search)
        {
            var searchResults = _context.Products.Where(p => p.ProducctName.Contains(search)).Include(p => p.Brand).
                Include(p => p.Category).Include(p => p.Feature).Include(p => p.Shape).Include(p => p.Type).ToPagedList(1, 99);
            // Pass the search results to the view
            return View("Index", searchResults);
        }







		[AuthenAdmin]


		[Route("")]
        [Route("Details")]

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Feature)
                .Include(p => p.Shape)
                .Include(p => p.Type)
                .Include(p => p.Images)
                .Include(p => p.Stocks)
                .FirstOrDefaultAsync(m => m.Pid == id);

            if (product == null)
            {
                return NotFound();
            }
            var stock = await _context.Stocks.Include(s => s.Color).Where(s => s.ProductId == product.Pid).ToListAsync();
            ViewData["stock"] = stock;

            return View(product);
        }







		[AuthenAdmin]

        //return view re attribute 
		// GET: Products/Create
		[Route("Create")]
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Bid", "Bname");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Cname");
            ViewData["FeatureId"] = new SelectList(_context.Features, "FeatureId", "FeatureName");
            ViewData["ShapeId"] = new SelectList(_context.Shapes, "ShapeId", "ShapeName");
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName");
            return View();
        }







        //add data to database
		[AuthenAdmin]

		[Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pid,ProducctName,Description,CategoryId,BrandId,Price,TypeId,ShapeId,FeatureId,Created,Updated,BestSeller")]
        Product product, List<Group4_GlassesShop.Models.DataModel.Image> images)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;

                    foreach (var error in errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        var propertyName = key;
                        Console.WriteLine(errorMessage + "_________" + propertyName);

                        // You can handle the error message and property name here
                        // For example, you can log them or display them to the user
                    }
                }
                ViewData["error image"] = "Please chose full images";
            }
            if (product.Price < 0)
            {
                ModelState.AddModelError("Price", "Price cannot be negative.");
            }
            if (product.ProducctName == null)
            {
                ModelState.AddModelError("ProducctName", "Name must have at least 9 characters.");
            }
            else if (product.ProducctName.Length <= 9)
            {
                ModelState.AddModelError("ProducctName", "Name must have at least 9 characters.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                //insert image
                foreach (var img in images)
                {
                    //img.ImageId = GenerateImageId();
                    img.Pid = product.Pid;
                    img.ImageUrl = "/Image/" + img.ImageUrl;
                    _context.Images.Add(img);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Bid", "Bname", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Cname", product.CategoryId);
            ViewData["FeatureId"] = new SelectList(_context.Features, "FeatureId", "FeatureName", product.FeatureId);
            ViewData["ShapeId"] = new SelectList(_context.Shapes, "ShapeId", "ShapeName", product.ShapeId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", product.TypeId);
            return View(product);
        }
        private int GenerateColorId()
        {
            // Retrieve the last used ColorId from the Color table
            var lastColor = _context.Colors.OrderByDescending(c => c.ColorId).FirstOrDefault();

            // Generate a new ColorId by incrementing the last used ColorId by 1
            int newColorId = lastColor != null ? lastColor.ColorId + 1 : 1;

            return newColorId;
        }
        private int GenerateStockId()
        {
            // Retrieve the last used ColorId from the Color table
            var lastColor = _context.Stocks.OrderByDescending(c => c.StockId).FirstOrDefault();

            // Generate a new ColorId by incrementing the last used ColorId by 1
            int newColorId = lastColor != null ? lastColor.StockId + 1 : 1;

            return newColorId;
        }
        private int GenerateOrderId()
        {
            // Retrieve the last used ColorId from the Color table
            var lastimg = _context.Orders.OrderByDescending(c => c.OrderId).FirstOrDefault();

            // Generate a new ColorId by incrementing the last used ColorId by 1
            int newImg = lastimg != null ? lastimg.OrderId + 1 : 1;

            return newImg;
        }




		[AuthenAdmin]

		[Route("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.Include(p => p.Stocks).Include(p => p.Images).FirstOrDefaultAsync(p => p.Pid == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Bid", "Bname", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Cname", product.CategoryId);
            ViewData["FeatureId"] = new SelectList(_context.Features, "FeatureId", "FeatureName", product.FeatureId);
            ViewData["ShapeId"] = new SelectList(_context.Shapes, "ShapeId", "ShapeName", product.ShapeId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", product.TypeId);
            return View(product);
        }
        //save data from form to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
		[AuthenAdmin]

		public async Task<IActionResult> Edit(int id, [Bind("Pid,ProducctName,Description,CategoryId,BrandId,Price,TypeId,ShapeId,FeatureId,Created,Updated,BestSeller")]
        Product product, List<Models.DataModel.Image> images)
        {

            if (!ModelState.IsValid)
            {
                ViewData["error image"] = "Please chose full images";
            }
            if (product.Price < 0)
            {
                ModelState.AddModelError("Price", "Price cannot be negative.");
            }
            if (product.ProducctName.Length <= 9)
            {
                ModelState.AddModelError("ProducctName", "Name must have at least 9 characters.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //pro
                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    //image
                    foreach (var img in images)
                    {
                        img.ImageUrl = "/Image/" + img.ImageUrl;
                        _context.Update(img);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Pid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Bid", "Bname", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Cname", product.CategoryId);
            ViewData["FeatureId"] = new SelectList(_context.Features, "FeatureId", "FeatureName", product.FeatureId);
            ViewData["ShapeId"] = new SelectList(_context.Shapes, "ShapeId", "ShapeName", product.ShapeId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", product.TypeId);
            return View(product);
        }



		// POST: Products/Delete/5
		[AuthenAdmin]

		[Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'GlassesShopContext.Products'  is null.");
            }
            //delete img
            var img = _context.Images.Where(x => x.Pid == id).ToList();
            if (img.Any()) { _context.RemoveRange(img); }

            //delete stock
            var stock = _context.Stocks.Where(x => x.ProductId == id).ToList();
            if (stock.Any()) { _context.RemoveRange(stock); }

            //delete product
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Pid == id)).GetValueOrDefault();
        }
    }
}
