using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Group4_GlassesShop.Controllers
{
    public class LoginController : Controller
	{
		private readonly ILogger<LoginController> _logger;
		private readonly IAccountRepository _accountRepository;

		public LoginController(ILogger<LoginController> logger, IAccountRepository accountRepository)
		{
			_logger = logger;
			_accountRepository = accountRepository;
		}

		#region Login
		/// <summary>
		/// GET: Login
		/// Returns the login view.
		/// </summary>
		/// <returns>Returns the login view.</returns>
		[HttpGet]
		public IActionResult Login()
		{
			AccountModel _user = new AccountModel();
			if (TempData.ContainsKey("SuccessMessage"))
			{
				ViewBag.SuccessMessage = TempData["SuccessMessage"];
			}
			return View(_user);
		}

		/// <summary>
		/// POST: Login
		/// Handles the login form submission.
		/// </summary>
		/// <param name="account">The AccountModel object representing the login form data.</param>
		/// <returns>Returns a redirection to the appropriate action based on the login result.</returns>
		[HttpPost]
		public IActionResult Login(AccountModel account)
		{
			var isExitsAcc = _accountRepository.IsExitsAccount(account);
			if (isExitsAcc != null)
			{
				string userJson = JsonConvert.SerializeObject(isExitsAcc);
				HttpContext.Session.SetString("User", userJson);
				if (isExitsAcc.Role == true)
				{
					// Redirect to the Dashboard action in the AdminDashboard controller (area: Admin)
					return RedirectToAction("Dashboard", "AdminDashboard", new { area = "Admin" });
				}
				else
				{
					// Redirect to the Index action in the Home controller (area: empty)
					return RedirectToAction("Index", "Home", new { area = "" });
				}
			}
			else
			{
				ViewBag.message = "Username or password not correct";
			}
			return View(account);
		}
		#endregion

		/// <summary>
		/// GET: Logout
		/// Logs out the user by removing the "User" key from the session.
		/// </summary>
		/// <returns>Returns a redirection to the Index action in the Home controller.</returns>
		public ActionResult Logout()
		{
			HttpContext.Session.Remove("User");
			return RedirectToAction("Index", "Home");
		}
	}
}
