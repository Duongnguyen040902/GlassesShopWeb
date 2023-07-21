using DemoWebFirstMVCCore;
using Group4_GlassesShop.Models.Authentication;
using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Group4_GlassesShop.Controllers.UserProfileManager
{
    public class FeedbackController : Controller
    {
        GlassesShopContext db = new GlassesShopContext();


        [Authentication]
        public IActionResult createFeedback(int productId, int orderId)

        {
            ViewData["productId"] = productId;
            ViewData["orderId"] = orderId;
            return View("CreateFeedback");
        }
        [Authentication]
        public IActionResult editFeedback(int productId, int orderId)
        {
            var account = HttpContext.Session.GetString("User");
            var customer = db.Customers.FirstOrDefault(c => c.AccountId == JsonConvert.DeserializeObject<Account>(account).AccountId);
            var feedback = db.Feedbacks.FirstOrDefault(f => f.ProductId == productId && f.CustomerId == customer.CustomerId);
            ViewData["productId"] = productId;
            ViewData["orderId"] = orderId;
            ViewData["content"] = feedback.Content;
            ViewData["rating"] = feedback.Rating;
            return View("EditFeedback", feedback);
        }
        [Authentication]
        [HttpPost]
        public IActionResult editFeedback(int productId, int orderId, String content, int rating)
        {
            var account = HttpContext.Session.GetString("User");
            var customer = db.Customers.FirstOrDefault(c => c.AccountId == JsonConvert.DeserializeObject<Account>(account).AccountId);
            var feedback = db.Feedbacks.FirstOrDefault(f => f.ProductId == productId && f.CustomerId == customer.CustomerId);
            ViewData["orderId"] = orderId;
            if (content == null || rating == 0)
            {
                ViewData["productId"] = productId;
                ViewData["errorMessage"] = "Content and rating can not be null";
                return View("EditFeedback");
            }
            feedback.Content = content;
            feedback.Rating = rating;
            db.SaveChanges();
            ViewData["message"] = "Edit feedback success";
            return View("EditFeedback");
        }
        [Authentication]
        [HttpPost]
        public IActionResult pushFeedback(int productId, int orderId, String content, int rating)
        {
            var account = HttpContext.Session.GetString("User");
            //get product by orderId

            var product = db.Products.FirstOrDefault(p => p.Pid == productId);
            var customer = db.Customers.FirstOrDefault(c => c.AccountId == JsonConvert.DeserializeObject<Account>(account).AccountId);

            var isFeedback = db.Feedbacks.FirstOrDefault(f => f.ProductId == productId && f.CustomerId == customer.CustomerId);
            ViewData["orderId"] = orderId;
            if (isFeedback != null)
            {
                ViewData["productId"] = productId;
                ViewData["errorMessage"] = "You have already feedback this product";
                return View("CreateFeedback");
            }

            if (content == null || rating == 0)
            {
                ViewData["productId"] = productId;
                ViewData["errorMessage"] = "Content and rating can not be null";
                return View("CreateFeedback");
            }

            // create new feedback
            var feedback = new Feedback();
            feedback.ProductId = product.Pid;

            feedback.CustomerId = customer.CustomerId;
            feedback.Content = content;
            feedback.Rating = rating;
            feedback.CreatedAt = DateTime.Now;
            feedback.IsApproved = true;
            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            ViewData["message"] = "Feedback success";



            return View("CreateFeedback");
        }

    }
}
