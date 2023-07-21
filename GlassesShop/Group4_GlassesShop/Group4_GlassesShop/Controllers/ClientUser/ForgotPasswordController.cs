using DemoWebFirstMVCCore;
using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Group4_GlassesShop.Controllers
{
	public class ForgotPasswordController : Controller
	{
		GlassesShopContext db = new GlassesShopContext();
		private readonly IEmailSender emailSender;
		public ForgotPasswordController(IEmailSender emailSender)
		{
			this.emailSender = emailSender;
		}
		#region Forgot
		/// <summary>
		/// GET: Forgot
		/// Returns the forgot password view.
		/// </summary>
		[HttpGet]
		public IActionResult Forgot()
		{
			return View();
		}

		/// <summary>
		/// POST: Forgot
		/// Handles the forgot password form submission.
		/// </summary>
		/// <param name="Email">The email entered by the user.</param>
		/// <returns>Returns a redirection to the MatchCodeForgot action if the email exists, otherwise returns the forgot password view with an error message.</returns>
		public async Task<IActionResult> Forgot(string Email)
		{
			var checkExitsEmail = db.Accounts.Where(e => e.Email == Email).FirstOrDefault();
			if (checkExitsEmail != null)
			{
				string accountJson = JsonConvert.SerializeObject(checkExitsEmail);
				HttpContext.Session.SetString("ForgotAccount", accountJson);
				string code = new RandomCode().GetRandomCode();
				HttpContext.Session.SetString("CodeForgot", "code");
				await emailSender.SendEmailAsync(Email, code);
				TempData["CodeForgot"] = code;
				return RedirectToAction("MatchCodeForgot");
			}
			else
			{
				ViewBag.message = "Not found your account! Please try again.";
			}
			return View();
		}

		/// <summary>
		/// GET: MatchCodeForgot
		/// Returns the match code view.
		/// </summary>
		/// <returns>Returns the match code view with a session timeout and code email if available.</returns>
		[HttpGet]
		public ActionResult MatchCodeForgot()
		{
			const int sessionTimeoutSeconds = 60;
			string codeEmail = TempData["CodeForgot"] as string;
			if (codeEmail != null)
			{
				HttpContext.Session.SetString("SaveCodeEmailForgot", codeEmail);
				DateTime sessionTimeout = DateTime.Now.AddSeconds(sessionTimeoutSeconds);
				HttpContext.Session.SetString("SessionTimeout", sessionTimeout.ToString("O"));
				ViewBag.SessionTimeoutSeconds = sessionTimeoutSeconds;
			}
			return View();
		}

		/// <summary>
		/// POST: MatchCodeForgot
		/// Handles the match code form submission.
		/// </summary>
		/// <param name="CodeForgot">The code entered by the user.</param>
		/// <returns>Returns a redirection to the ChangePassWordForgot action if the code matches and is within the session timeout, otherwise returns a redirection to the MatchCodeForgot action with an error message.</returns>
		[HttpPost]
		public ActionResult MatchCodeForgot(string CodeForgot)
		{
			string EmailCodeFogot = HttpContext.Session.GetString("SaveCodeEmailForgot");
			string sessionTimeoutString = HttpContext.Session.GetString("SessionTimeout");
			DateTime sessionTimeout = DateTime.Parse(sessionTimeoutString);
			if (CodeForgot.Equals(EmailCodeFogot) && DateTime.Now <= sessionTimeout)
			{
				return RedirectToAction("ChangePassWordForgot");
			}
			else
			{
				TempData["ErrorMessage"] = "Code does not match! Please try again.";
				return RedirectToAction("MatchCodeForgot");
			}
		}

		/// <summary>
		/// GET: ChangePassWordForgot
		/// Returns the change password view.
		/// </summary>
		/// <returns>Returns the change password view.</returns>
		[HttpGet]
		public ActionResult ChangePassWordForgot()
		{
			return View();
		}

		/// <summary>
		/// POST: ChangePassWordForgot
		/// Handles the change password form submission.
		/// </summary>
		/// <param name="FogotPass">The new password entered by the user.</param>
		/// <param name="RePass">The confirm password entered by the user.</param>
		/// <returns>Returns a redirection to the login view if the password change is successful, otherwise returns the change password view with an error message.</returns>
		[HttpPost]
		public ActionResult ChangePassWordForgot(string FogotPass, string RePass)
		{
			string accountJson = HttpContext.Session.GetString("ForgotAccount");
			var AccountForgot = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(accountJson);

			if (string.IsNullOrEmpty(FogotPass) || string.IsNullOrEmpty(RePass))
			{
				TempData["ErrorMessage"] = "Please enter both new password and confirm password.";
				return View();
			}

			if (FogotPass.Equals(RePass))
			{
				if (string.IsNullOrEmpty(FogotPass) || FogotPass.Length < 6)
				{
					TempData["ErrorMessage"] = "New password must be at least 6 characters.";
					return View();
				}

				bool hasUpperCase = false;
				bool hasLowerCase = false;
				bool hasDigit = false;
				bool hasSpecialChar = false;
				foreach (char c in FogotPass)
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

				if (!hasUpperCase || !hasLowerCase || !hasDigit || !hasSpecialChar)
				{
					TempData["ErrorMessage"] = "New password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
					return View();
				}

				if (AccountForgot != null)
				{
					AccountForgot.Password = FogotPass;
					db.Accounts.Update(AccountForgot);
					db.SaveChanges();
					TempData["SuccessMessage"] = "Change Password Successfully!";
					HttpContext.Session.Remove("ForgotAccount");
					return RedirectToAction("Login", "Login");
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
#endregion

	}
}
