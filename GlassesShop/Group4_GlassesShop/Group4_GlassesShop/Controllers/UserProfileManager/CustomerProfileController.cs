using Group4_GlassesShop.Models.Authentication;
using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.UpdateProfile;
using Group4_GlassesShop.Models.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Group4_GlassesShop.Controllers.UserProfileManager
{
    public class CustomerProfileController : Controller
    {
        GlassesShopContext db = new GlassesShopContext();


        [Authentication]
        /// <summary>
        /// Retrieves the user profile information.
        /// </summary>
        /// <returns>The user profile view.</returns>
        public IActionResult UserProfile()
        {
            if (TempData.ContainsKey("SuccessUpdate"))
            {
                ViewBag.SuccessUpdate = TempData["SuccessUpdate"];
            }
            if (TempData.ContainsKey("ChangeSuccess"))
            {
                ViewBag.SuccessUpdate = TempData["ChangeSuccess"];
            }
            string userJson = HttpContext.Session.GetString("User");

            if (userJson == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                // Get the accountJson value from session 
                var isExitsUser = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(userJson); // Deserialize the user information from the JSON string
                var viewCustomer = db.Customers.FirstOrDefault(c => c.AccountId == isExitsUser.AccountId);
                string CustomerSession = JsonConvert.SerializeObject(viewCustomer);
                HttpContext.Session.SetString("CustomerSession", CustomerSession);
                return View(viewCustomer);
            }
        }

       
        /// <summary>
        /// GET
        /// Displays the update profile form.
        /// </summary>
        /// <returns>The update profile view.</returns>
        
        [Authentication]
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                string CustomerJson = HttpContext.Session.GetString("CustomerSession"); // Retrieve the customer information from the session
                var GetCustomer = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Customer>(CustomerJson);
                var GetAccount = db.Accounts.FirstOrDefault(a => a.AccountId == GetCustomer.AccountId);
                ViewBag.Email = GetAccount.Email;

                // // Initialize an UpdateProfileModel object with the customer's data
                var updateProfileModel = new UpdateProfileModel
                {
                    CustomerName = GetCustomer.Name,
                    PhoneNumber = GetCustomer.PhoneNumber,
                    AvatarUrl = GetCustomer.AvatarUrl
                };
                string avatar = JsonConvert.SerializeObject(GetCustomer.AvatarUrl);
                HttpContext.Session.SetString("avatar", avatar);

                return View(updateProfileModel);
            }
        }

        /// <summary>
        /// POST
        /// Updates the customer's profile.
        /// </summary>
        /// <param name="reModel">The update profile model.</param>
        /// <returns>The action result.</returns>
        [Authentication]
        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UpdateProfileModel reModel)
        {
            // Call the validator class
            var validator = new UpdateProfileValidation();
            var validationResult = await validator.ValidateAsync(reModel);

            // Check validation
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)   // If validation fails, add model errors to the ModelState and return the view with the updated model
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }

                return View(reModel);
            }

            string GetCustomer = HttpContext.Session.GetString("CustomerSession");
            var CustomerSession = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Customer>(GetCustomer);
            var CustomerUpdate = db.Customers.FirstOrDefault(c => c.CustomerId == CustomerSession.CustomerId);
            string? getAvatar = HttpContext.Session.GetString("avatar"); //get old avatar by session
            string? avatarUrl = getAvatar.Trim('"'); //use Substring in combination with Trim to remove quotes

            if (CustomerUpdate != null)
            {
                // Check if a new avatar image was selected
                if (reModel.AvatarUrl != null && reModel.AvatarUrl.Length > 0)
                {
                    CustomerUpdate.AvatarUrl = "/Avatar/" + reModel.AvatarUrl;
                }
                else
                {   
                    CustomerUpdate.AvatarUrl = avatarUrl; // Set the default avatar image URL when no new image is selected
                }
                CustomerUpdate.Name = reModel.CustomerName;
                CustomerUpdate.PhoneNumber = reModel.PhoneNumber;
                
                db.Customers.Update(CustomerUpdate); // Update the customer in the database and save the changes
                db.SaveChanges();
                TempData["SuccessUpdate"] = "Update Successfully!";
                return RedirectToAction("UserProfile", "CustomerProfile");
            }

            return View("UpdateProfile");
        }
    }
}

