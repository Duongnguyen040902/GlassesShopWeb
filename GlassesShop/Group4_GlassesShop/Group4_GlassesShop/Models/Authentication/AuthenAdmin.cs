using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Group4_GlassesShop.Models.DataModel;
using Newtonsoft.Json;

namespace Group4_GlassesShop.Models.Authentication
{
	public class AuthenAdmin : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{

			string userJson = context.HttpContext.Session.GetString("User");



			if (userJson == null)
			{
				context.Result = new RedirectToRouteResult(
					new RouteValueDictionary
					{
						{ "Controller", "Login" },
						{ "Action", "Login" }
					});
			}
			else
			{
				var isExitsUser = JsonConvert.DeserializeObject<AccountModel>(userJson);
				if (isExitsUser.Role == false)
				{
					context.Result = new RedirectToRouteResult(
						new RouteValueDictionary
						{
						{ "Controller", "Home" },
						{ "Action", "Index" }
						});
				}
			}
		}
	} 
}


//public override void OnActionExecuting(ActionExecutingContext context)
//{

//    string userJson = context.HttpContext.Session.GetString("User");

//    var isExitsUser = JsonConvert.DeserializeObject<AccountModel>(userJson);

//    if (userJson == null)
//    {
//        context.Result = new RedirectToRouteResult(
//            new RouteValueDictionary
//            {
//                        { "Controller", "Login" },
//                        { "Action", "Login" }
//            });
//    }
//    else if (isExitsUser.Role == false)
//    {
//        context.Result = new RedirectToRouteResult(
//            new RouteValueDictionary
//            {
//                        { "Controller", "Home" },
//                        { "Action", "Index" }
//            });
//    }
//}