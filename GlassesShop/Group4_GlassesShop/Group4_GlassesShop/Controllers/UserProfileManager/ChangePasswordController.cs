using DemoWebFirstMVCCore;
using Group4_GlassesShop.Models.Authentication;
using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Group4_GlassesShop.Controllers.UserProfileManager
{
    public class ChangePasswordController : Controller
    {
        GlassesShopContext db = new GlassesShopContext();
        private readonly IEmailSender emailSender;
        public ChangePasswordController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        /// <summary>
        /// GET
        /// Displays the send code form.
        /// </summary>
        /// <returns>The send code view.</returns>
        [Authentication]
        [HttpGet]
        public IActionResult SendCode()
        {
            // Retrieve the user information from the session
            string userJson = HttpContext.Session.GetString("User");
            var GetAccount = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(userJson);

            if (userJson == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(GetAccount);
            }
        }

        /// <summary>
        /// POST
        /// Sends the code to the user's email.
        /// </summary>
        /// <param name="account">The user's account.</param>
        /// <returns>The action result.</returns>  
        [Authentication]
        [HttpPost]
        public async Task<IActionResult> SendCode(string account)
        {
            account = HttpContext.Session.GetString("User"); 
            var GetAccount = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(account);

            if (GetAccount != null)
            {
                HttpContext.Session.SetString("SendEmailAccount", account); // Store the user's email in the session
                string code = new RandomCode().GetRandomCode();  // Generate a random code
                HttpContext.Session.SetString("CodeChange", "code");
                await emailSender.SendEmailAsync(GetAccount.Email, code);  // Send the code to the user's email
                TempData["CodeChange"] = code;
                HttpContext.Session.SetString("check", "flase"); // Set a session variable to track if the code was sent successfully
                return RedirectToAction("MatchCodeChange");
            }
            else
            {
                ViewBag.message = "Not found your account ! pls try again";
            }
            return View(GetAccount);
        }

        /// <summary>
        /// GET
        /// Displays the match code change form.
        /// </summary>
        /// <returns>The match code change view.</returns>
        [Authentication]
        [HttpGet]
        public ActionResult MatchCodeChange()
        {
            string codeEmail = TempData["CodeChange"] as string; // Retrieve the code from TempData and store it in the session
            if (codeEmail != null)
            {
                HttpContext.Session.SetString("SaveCodeEmailChange", codeEmail);
            }
            return View();
        }

        /// <summary>
        /// POST
        /// Matches the entered code with the code sent via email.
        /// </summary>
        /// <param name="CodeChange">The entered code.</param>
        /// <returns>The action result.</returns>
        [Authentication]
        [HttpPost]
        public ActionResult MatchCodeChange(string CodeChange)
        {
            string EmailCodeChange = HttpContext.Session.GetString("SaveCodeEmailChange"); // Retrieve the code sent via email from the session
            if (string.IsNullOrEmpty(CodeChange))
            {
                TempData["ErrorMessage"] = "Code must be required"; 
                return RedirectToAction("MatchCodeChange");
            }
            else if (CodeChange.Equals(EmailCodeChange))
            {
                HttpContext.Session.SetString("check", "true");
                return RedirectToAction("ChangePassword");
            }
            else
            {
                TempData["ErrorMessage"] = "Code does not match! Please try again.";
                return RedirectToAction("MatchCodeChange");
            }
        }

        /// <summary>
        /// GET
        /// Displays the change password form.
        /// </summary>
        /// <returns>The change password view.</returns>
        [Authentication]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("check").Equals("true"))  // Check if the session variable "check" is set to "true"
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        /// <summary>
        /// POST
        /// Changes the user's password.
        /// </summary>
        /// <param name="NewPass">The new password.</param>
        /// <param name="RePass">The confirm password.</param>
        /// <returns>The action result.</returns>
        [Authentication]
        [HttpPost]
        public ActionResult ChangePassword(string NewPass, string RePass)
        {
            string accountsession = HttpContext.Session.GetString("User");
            var ChangePassAccount = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(accountsession);

            if (string.IsNullOrEmpty(NewPass) || string.IsNullOrEmpty(RePass))
            {
                TempData["ErrorMessage"] = "Please enter both new password and confirm password.";
                return View();
            }

            if (NewPass.Equals(RePass))
            {
                if (NewPass.Equals(ChangePassAccount.Password))
                {
                    TempData["ErrorMessage"] = "Please enter a new password!";
                    return View();
                }

                if (string.IsNullOrEmpty(NewPass) || NewPass.Length < 6)
                {
                    TempData["ErrorMessage"] = "New password must be at least 6 characters.";
                    return View();
                }

                bool hasUpperCase = false;
                bool hasLowerCase = false;
                bool hasDigit = false;
                bool hasSpecialChar = false;

                // Iterate through each character in the NewPass string
                foreach (char c in NewPass)
                {
                    if (char.IsUpper(c))
                    {
                        hasUpperCase = true;
                    }
                    else if (char.IsLower(c))
                    {
                        hasLowerCase = true;
                    }
                    else if (char.IsDigit(c))
                    {
                        hasDigit = true;
                    }
                    else if (char.IsSymbol(c) || char.IsPunctuation(c))
                    {
                        hasSpecialChar = true;
                    }
                }

                // If the new password does not meet the complexity requirements, set an error message in TempData and return the change password view
                if (!hasUpperCase || !hasLowerCase || !hasDigit || !hasSpecialChar)
                {
                    TempData["ErrorMessage"] = "New password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
                    return View();
                }

                if (ChangePassAccount != null)
                {
                    // Update the user's password with the new password and save the changes
                    ChangePassAccount.Password = NewPass;
                    db.Accounts.Update(ChangePassAccount);
                    db.SaveChanges();
                    TempData["ChangeSuccess"] = "Change Password Successfully!";
                    return RedirectToAction("UserProfile", "ViewProfile");
                }
                else
                {
                    TempData["ErrorMessage"] = "Account not found. Please try again.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Password does not match. Please try again.";
            }

            return View();
        }
    }
}
