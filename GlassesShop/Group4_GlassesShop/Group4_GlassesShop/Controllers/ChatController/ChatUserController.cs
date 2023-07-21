using Group4_GlassesShop.Models.Authentication;
using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Diagnostics;

namespace Group4_GlassesShop.Controllers.ChatController
{
    public class ChatUserController : Controller
    {


        private readonly GlassesShopContext _context;

        public ChatUserController(GlassesShopContext context)
        {
            _context = context;
        }
      

      
        public IActionResult Index()
        {
			int susu=99999;
			string userJson = HttpContext.Session.GetString("User");
            if (userJson != null)
            {
                var isExitsUser = JsonConvert.DeserializeObject<Account>(userJson);
                susu = isExitsUser.AccountId;
				HttpContext.Session.SetInt32("accid", susu);
				var ChatChose = _context.Chats.FirstOrDefault(c => c.AccId == susu);
                if(ChatChose == null) { return PartialView("_ChatUser"); }
				var chatlistdetail = _context.ChatDetails.Where(d => d.ChatId == ChatChose.ChatId).ToList();
				TempData["chatlistdetail"] = chatlistdetail;
			}
			

            return PartialView("_ChatUser");
        }


		private int GenerateChatId()
		{
			// Retrieve the last used ColorId from the Color table
			var lastColor = _context.Chats.OrderByDescending(c => c.ChatId).FirstOrDefault();

			// Generate a new ColorId by incrementing the last used ColorId by 1
			int newColorId = lastColor != null ? lastColor.ChatId + 1 : 1;

			return newColorId;
		}
		[HttpPost]
		[Authentication]

		public IActionResult Send([Bind("Text,TimeChat,ChatId,Role")] ChatDetail chatdetail)
        {

            //chat detail
            int number = HttpContext.Session.GetInt32("accid") ?? 9999;//get accid
            var ChatforID = _context.Chats.FirstOrDefault(d => d.AccId == number);//get chats by accid
            if (ChatforID != null)
            {
                int chatid = ChatforID.ChatId;
				chatdetail.ChatId = chatid;

                DateTime hh = DateTime.Now;
                chatdetail.TimeChat = hh;
                DateTime gg = chatdetail.TimeChat;
                Debug.WriteLine(gg.ToString());
            }
            //chatted

            //non-chatted
            if(ChatforID == null)
            {
                Chat chat = new Chat();
                chat.AccId = number;
                chat.ChatId = GenerateChatId();
                chatdetail.ChatId = chat.ChatId;
				chatdetail.TimeChat = DateTime.Now;
				_context.Chats.Add(chat);
                _context.SaveChanges();
            }


			if (ModelState.IsValid)
            {
                _context.ChatDetails.Add(chatdetail);
                _context.SaveChanges();
            }
            
            TempData["id"] = number;
			return Redirect(Request.Headers["Referer"].ToString());
		}
    }
}

