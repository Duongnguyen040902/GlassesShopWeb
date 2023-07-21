using Group4_GlassesShop.Models.Authentication;
using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.Entity;

namespace Group4_GlassesShop.Areas.Admin.Controllers.ChatManagement
{

	[Area("admin")]
	[Route("admin")]
	[Route("admin/Chat")]
	public class ChatController : Controller
	{

		private readonly GlassesShopContext _context;

		public ChatController(GlassesShopContext context)
		{
			_context = context;
		}
		//like index
		[Route("gg")]
		[AuthenAdmin]

		public IActionResult gg()
		{
			var ChatList = _context.Chats
						.Include(c => c.Acc)
						.Include(c => c.Acc.Customer)
						.Select(c => new
						{
							CustomerName = c.Acc.Customer.Name,
							CustomerAvatarUrl = c.Acc.Customer.AvatarUrl,
							AccID = c.Acc.AccountId

						})
						.ToList();

			//TempData["dungsai"] = 1;

			ViewBag.listUser = ChatList;
			
			if (TempData["chatlistdetail"] != null)
			{
				string image = TempData["image"] as string;
				ViewBag.image = image;
				string nameCus = TempData["nameCus"] as string;
				ViewBag.nameCus = nameCus;
				var data = TempData["chatlistdetail"] as string;
				var chatlistdetail = JsonConvert.DeserializeObject<List<Group4_GlassesShop.Models.DataModel.ChatDetail>>(data);
				return View("Index1", chatlistdetail);
			}
			else { return View("Index1"); }

		}
		[AuthenAdmin]

		//like load detail chat
		[Route("Susu")]
		[HttpPost]
		public IActionResult Susu(int susu)
		{
			//if (TempData["id"] != null)
			//{
			//	susu = (int)TempData["id"];
			//}
			HttpContext.Session.SetInt32("accid", susu);
			var forimage = _context.Customers.FirstOrDefault(c => c.AccountId == susu);
			TempData["image"] = forimage.AvatarUrl;
			TempData["nameCus"] = forimage.Name;
			var ChatChose = _context.Chats.FirstOrDefault(c => c.AccId == susu);
			var chatlistdetail = _context.ChatDetails.Where(d => d.ChatId == ChatChose.ChatId).ToList();
			var serializedData = JsonConvert.SerializeObject(chatlistdetail);
			TempData["chatlistdetail"] = serializedData;

			return RedirectToAction("gg", "Chat", new { area = "admin" });
		}
		[AuthenAdmin]


		//for send
		[Route("Susu")]
		[HttpGet]
		public IActionResult SusuG(int susu)
		{
			if (TempData["id"] != null)
			{
				susu = (int)TempData["id"];
			}
			HttpContext.Session.SetInt32("accid", susu);
			var ChatChose = _context.Chats.FirstOrDefault(c => c.AccId == susu);
			var chatlistdetail = _context.ChatDetails.Where(d => d.ChatId == ChatChose.ChatId).ToList();
			var serializedData = JsonConvert.SerializeObject(chatlistdetail);
			TempData["chatlistdetail"] = serializedData;

			return RedirectToAction("gg", "Chat", new { area = "admin" });
		}
		[AuthenAdmin]

		[Route("Send")]
		[HttpPost]
		public IActionResult Send([Bind("Text,TimeChat,ChatId,Role")] ChatDetail chatdetail)
		{
			//chat detail
			int number = HttpContext.Session.GetInt32("accid") ?? 9999;
			int chatid = _context.Chats.FirstOrDefault(d => d.AccId == number).ChatId;
			chatdetail.ChatId = chatid;
			chatdetail.TimeChat = DateTime.Now;

			if (ModelState.IsValid)
			{
				_context.ChatDetails.Add(chatdetail);
				 _context.SaveChanges();

			}
			TempData["id"] = number;
			return RedirectToAction("SusuG", "Chat", new { area = "admin", susu = number });
		}

	
		private int GenerateChatId()
		{
			// Retrieve the last used ColorId from the Color table
			var lastColor = _context.Colors.OrderByDescending(c => c.ColorId).FirstOrDefault();

			// Generate a new ColorId by incrementing the last used ColorId by 1
			int newColorId = lastColor != null ? lastColor.ColorId + 1 : 1;

			return newColorId;
		}
	}
}
