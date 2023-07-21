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
    [Route("admin/Feedback")]
    public class FeedbackController : Controller
    {

        private readonly GlassesShopContext _context;

        public FeedbackController(GlassesShopContext context)
        {
            _context = context;
        }
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index(int page = 1)
        {
            // init page size include 20 items
            var pageSize = 20;
            var total = _context.Feedbacks.Count();
            ViewBag.PageCount = Math.Ceiling((double)total / pageSize);
            ViewBag.Page = page;
            var feedback = _context.Feedbacks.Include(a => a.Customer)
                .Include(a => a.Product)
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return View(feedback);
        }



        [Route("Details")]
        public async Task<IActionResult> Details(int? id, string? message)
        {
            if (id == null)
            {
                return NotFound();
            }
            var feedback = await _context.Feedbacks.Include(a => a.Customer)
               .Include(a => a.Product)
               .Include(a => a.Product.Images)
               .Include(a => a.Product.Category)
               .Include(a => a.Product.Brand)
               .FirstOrDefaultAsync(a => a.FeedbackId == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        [Route("Block")]
        public async Task<IActionResult> Block(int? id)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(o => o.FeedbackId == id);
            if (feedback != null)
            {
                feedback.IsApproved = false;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = feedback.FeedbackId, message = "Feedback has been blocked" });
        }

        [Route("Unblock")]
        public async Task<IActionResult> Unblock(int? id)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(o => o.FeedbackId == id);
            if (feedback != null)
            {
                feedback.IsApproved = true;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = feedback.FeedbackId, message = "Feedback has been unblocked" });
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



            return RedirectToAction(nameof(Index));
        }

        //// POST: Accounts/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("AccountId,Email,Password,Role,Active")] Account account)
        //{
        //    if (id != account.AccountId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(account);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AccountExists(account.AccountId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(account);
        //}

        // GET: Accounts/Delete/5

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
