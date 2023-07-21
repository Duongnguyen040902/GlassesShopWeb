using Microsoft.AspNetCore.Mvc;
using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Register;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DemoWebFirstMVCCore;
using System.Security.Principal;
using Group4_GlassesShop.Models.Validation;
using Group4_GlassesShop.Models.Interface;
using Group4_GlassesShop.Infrastructure;

namespace Group4_GlassesShop.Controllers
{
    public class RegisterController : Controller
	{
		private readonly IEmailSender emailSender;
		private readonly IAccountRepository _accountRepository;

		public RegisterController(IEmailSender emailSender, IAccountRepository accountRepository)
		{
			this.emailSender = emailSender;
			_accountRepository = accountRepository;

		}
		#region Register
		/// <summary>
		/// GET: Register
		/// Returns the registration view.
		/// </summary>
		/// <returns>Returns the registration view.</returns>
		[HttpGet]
		public ActionResult Register()
		{
			RegisterModel _register = new RegisterModel();
			return View(_register);
		}

		/// <summary>
		/// POST: Register
		/// Handles the registration form submission.
		/// </summary>
		/// <param name="reModel">The RegisterModel object representing the registration form data.</param>
		/// <returns>Returns a redirection to the ConfirmEmailCode action.</returns>
		[HttpPost]
		public async Task<ActionResult> Register(RegisterModel reModel)
		{
			// Call RegisterValidator class
			var validator = new RegisterValidator();
			var validationResult = await validator.ValidateAsync(reModel);

			// Check validation
			if (!validationResult.IsValid)
			{
				foreach (var error in validationResult.Errors)
				{
					ModelState.AddModelError("", error.ErrorMessage);
				}
				return View(reModel);
			}

			var checkEmail = _accountRepository.IsExitsEmail(reModel);
			if (checkEmail)
			{
				ModelState.AddModelError("Email", "Email already exists! Please try another email.");
				return View(reModel);
			}

			// Save Account to session and generate email code
			var NewAccount = new Group4_GlassesShop.Models.DataModel.AccountModel(reModel.Email, reModel.Password, false, false);
			string accountJson = JsonConvert.SerializeObject(NewAccount);
			HttpContext.Session.SetString("NewAccount", accountJson);

			var NewCustomer = new Group4_GlassesShop.Models.DataModel.Customer(reModel.Name, reModel.PhoneNumber, "/Avatar/DefaultAvatar.png");
			string customerJson = JsonConvert.SerializeObject(NewCustomer);
			HttpContext.Session.SetString("NewCustomer", customerJson);

			string code = new RandomCode().GetRandomCode();
			HttpContext.Session.SetString("codeEmail", code);

			await emailSender.SendEmailAsync(NewAccount.Email, code);
			TempData["codeEmail"] = code;

			return RedirectToAction("ConfirmEmailCode");
		}

		/*
		 If Email not exits and 
		 Regex is correct
		 RedirectToAction Controller ConfirmEmailCode
		*/
		[HttpGet]
		public ActionResult ConfirmEmailCode()
		{
			const int sessionTimeoutSeconds = 60;
			string codeEmail = TempData["codeEmail"] as string;
			HttpContext.Session.SetString("SaveCodeEmail", codeEmail);
			DateTime sessionTimeout = DateTime.Now.AddSeconds(sessionTimeoutSeconds);
			HttpContext.Session.SetString("SessionTimeout", sessionTimeout.ToString("O"));
			ViewBag.SessionTimeoutSeconds = sessionTimeoutSeconds;
			return View();
		}

		[HttpPost]
		public ActionResult ConfirmEmailCode(string code, bool countdownActive)
		{
			string codeEmail = HttpContext.Session.GetString("SaveCodeEmail");
			string accountJson = HttpContext.Session.GetString("NewAccount");
			var account = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.AccountModel>(accountJson);
			string customerJson = HttpContext.Session.GetString("NewCustomer");
			var customer = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Customer>(customerJson);
			string sessionTimeoutString = HttpContext.Session.GetString("SessionTimeout");

			DateTime sessionTimeout = DateTime.Parse(sessionTimeoutString);

			if (codeEmail != null && codeEmail.Equals(code) && DateTime.Now <= sessionTimeout)
			{
				_accountRepository.AddAccount(account, customer);
				TempData["SuccessMessage"] = "Create Successfully! Login now go to shopping";
				return RedirectToAction("Login", "Login");
			}
			else
			{
				int initialTime = 60; // Số giây ban đầu (60 giây)
				int remainingTime = initialTime - (int)(DateTime.Now - sessionTimeout).TotalSeconds;
				if (remainingTime < 0)
				{
					remainingTime = 0;
				}
				TempData["ErrorMessage"] = "Code does not match! Please try again.";
				ViewBag.ErrorMessage = TempData["ErrorMessage"];
				ViewBag.CountdownActive = countdownActive;
				ViewBag.InitialTime = remainingTime;
			}

			return View("ConfirmEmailCode");
		}

		[HttpPost]
		public async Task<IActionResult> ResendCodeAsync()
		{
			string newCode = new RandomCode().GetRandomCode(); // Tạo mã mới
			string accountJson = HttpContext.Session.GetString("NewAccount"); // Lấy thông tin tài khoản từ session
			var newAccount = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(accountJson);
			string email = newAccount.Email; // Lấy email từ tài khoản

			await emailSender.SendEmailAsync("achienvk789@gmail.com", newCode); // Gửi mã mới đến email của người dùng

			int countdownSeconds = 60; // Số giây countdown mới

			return Json(new { code = newCode, countdownSeconds });
		}
		#endregion
	}
}
