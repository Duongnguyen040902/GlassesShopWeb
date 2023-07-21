using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group4_GlassesShop.Models.DataModel;

namespace Group4_GlassesShop.Controllers
{
	[Area("admin")]
	[Route("admin")]
	[Route("admin/Accounts")]
	public class AccountsController : Controller
    {
		
        private readonly GlassesShopContext _context;

        public AccountsController(GlassesShopContext context)
        {
            _context = context;
        }
		[Route("")]
		[Route("Index")]
		// GET: Accounts
        //get list account
		public async Task<IActionResult> Index()
        {
              return _context.Accounts != null ? 
                          View(await _context.Accounts.ToListAsync()) :
                          Problem("Entity set 'GlassesShopContext.Accounts'  is null.");
        }


		[Route("Details")]

		// GET: Accounts/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }
			var account = await _context.Accounts.Include(a => a.Customer).FirstOrDefaultAsync(a => a.AccountId == id);
            var add = await _context.Addresses.
                Include(a => a.Customer).
				Include(a => a.DistrictsCodeNavigation).
				Include(a => a.ProvincesCodeNavigation).
				Include(a => a.WardsCodeNavigation).

				FirstOrDefaultAsync(a => a.CustomerId == account.Customer.CustomerId);
            if (add != null)
            {
                var addd = add.WardsCodeNavigation.WardsName + add.DistrictsCodeNavigation.DistrictName + add.ProvincesCodeNavigation.ProvinName;
                ViewData["full_add"] = addd.ToString();
            }
			if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

		[Route("Edit")]

		// GET: Accounts/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
			var account = await _context.Accounts.FirstOrDefaultAsync(o => o.AccountId == id);
			if (account != null)
			{
				account.Active = true;
			}
			await _context.SaveChangesAsync();

			return Redirect(Request.Headers["Referer"].ToString());
		}

       

        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.Include(a => a.Customer).FirstOrDefaultAsync(a => a.AccountId == id);

			if (account != null)
			{
				var customer = account.Customer;
				if (customer != null)
				{
					_context.Customers.Remove(customer);
				}
				_context.Accounts.Remove(account);
				await _context.SaveChangesAsync();
			}


			return RedirectToAction(nameof(Index));
        }


        private bool AccountExists(int id)
        {
          return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
