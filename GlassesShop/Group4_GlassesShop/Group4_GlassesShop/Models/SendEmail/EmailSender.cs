using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Group4_GlassesShop.Infrastructure;
using System.Security.Principal;
using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Cart;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Data.SqlClient;

namespace DemoWebFirstMVCCore
{
    public class EmailSender : IEmailSender
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GlassesShopContext _context;

        public EmailSender(IHttpContextAccessor httpContextAccessor,GlassesShopContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context= context;
        }

        public Task SendEmailAsync(string email, string message)
        {
            var client = new SmtpClient("smtp.gmail.com") 
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("quannahe164025@gmail.com", "xikyyrotuijfsvaz")
            };
            var mailMessage = new MailMessage()
            {
                From = new MailAddress("GlassesShop@gmail.com"),
                To = { new MailAddress(email) },
                Subject = "Glasess Shope ",
                IsBodyHtml = true,
                Body = @"    <table class=""body-wrap"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; background-color: #f6f6f6; margin: 0;"" bgcolor=""#f6f6f6"">
        <tbody>
            <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                <td style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0;"" valign=""top""></td>
                <td class=""container"" width=""600"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; display: block !important; max-width: 600px !important; clear: both !important; margin: 0 auto;"" valign=""top"">
                    <div class=""content"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; max-width: 600px; display: block; margin: 0 auto; padding: 20px;"">
                        <table class=""main"" width=""100%"" cellpadding=""0"" cellspacing=""0"" itemprop=""action"" itemscope="""" itemtype=""http://schema.org/ConfirmAction"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; border-radius: 3px; margin: 0; border: none;"">
                            <tbody><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                <td class=""content-wrap"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0;padding: 30px;border: 3px solid #67a8e4;border-radius: 7px; background-color: #fff;"" valign=""top"">
                                    <meta itemprop=""name"" content=""Confirm Email"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                        <tbody><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                            <td class=""content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"" valign=""top"">
                                                Hi <br>
                                                Thank you for registering with us. Please confirm your registration by entering the verification code below:
                                            </td>
                                        </tr>
                                        <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                            <td class=""content-block"" itemprop=""handler"" itemscope="""" itemtype=""http://schema.org/HttpActionHandler"" style=""font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"">
                                                <h4 style=""margin: 0; font-size: 14px; font-weight: normal; line-height: 1.4; color: #FFF; display: inline-block; background-color: #f06292; padding: 8px 16px; border-radius: 5px;"">" + message + @"</h4>
                                            </td>                                            
                                        </tr>
                                        <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                            <td class=""content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"" valign=""top"">
                                                <b>By Glasses Shop</b> 
                                            </td>
                                        </tr>
                                    </tbody></table>
                                </td>
                            </tr>
                        </tbody></table>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>   "
            };
             return client.SendMailAsync(mailMessage);
        }

        public Task SendEmailOrderAsync(string email)
        {
            string nameCus = "";
            string phoneCus = "";
            string AddressCus = "";
            var client = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("quannahe164025@gmail.com", "xikyyrotuijfsvaz")
            };

            var httpContext = _httpContextAccessor.HttpContext;
            int? orderCus = httpContext.Session.GetInt32("orderCus");
            if (orderCus != 0)
            {
                var OrderCus = _context.Orders.Where(o => o.OrderId == orderCus).FirstOrDefault();
                var Address = _context.Addresses.FirstOrDefault(a => a.AddressId == OrderCus.AddressId);
                var CusOrder = _context.Customers.FirstOrDefault(c => c.CustomerId ==  OrderCus.CustomerId);
                AddressCus += Address.AddDetails;
                nameCus += CusOrder.Name;
                phoneCus += CusOrder.PhoneNumber;
            }

            var mailMessage = new MailMessage()
            {

                From = new MailAddress("GlassesShop@gmail.com"),
                To = { new MailAddress(email) },
                Subject = "Glasess Shope ",
                IsBodyHtml = true,
                Body = BuildEmailBody(nameCus ,phoneCus , AddressCus,email)
            };
            return client.SendMailAsync(mailMessage);
        }

        //public List<OrderModel> GetOrderDetails(int orderCus)
        //{
        //    var orderList = _context.Set<OrderModel>()
        //    .FromSqlRaw("EXEC GetOrderItemsByOrderCus @OrderCus", new SqlParameter("@OrderCus", orderCus))
        //    .ToList();
        //    return orderList;
        //}

        private string BuildEmailBody(string nameCus , string phoneCus , string AddressCus , string email )
        {
            StringBuilder bodyBuilder = new StringBuilder();

            bodyBuilder.AppendLine("<html>");
            bodyBuilder.AppendLine("<body>");
            bodyBuilder.AppendLine(@"<center style=""width: 100%; background-color: #f1f1f1;"">");
            bodyBuilder.AppendLine(@"<div style=""display: none; font-size: 1px;max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;"">");
            bodyBuilder.AppendLine(@"&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;");
            bodyBuilder.AppendLine(@"</div>");
            bodyBuilder.AppendLine(@"<div style=""max-width: 600px; margin: 0 auto;"" class=""email-container"">");
            bodyBuilder.AppendLine(@"<!-- BEGIN BODY -->");
            bodyBuilder.AppendLine(@"<table align=""center"" role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin: auto;"">");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<td valign=""top"" class=""bg_white"" style=""padding: 1em 2.5em 0 2.5em;"">");
            bodyBuilder.AppendLine(@"<table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<td class=""logo"" style=""text-align: left;"">");
            bodyBuilder.AppendLine(@"<img src=""logo.png"" alt="""">");
            bodyBuilder.AppendLine(@"</td>");
            bodyBuilder.AppendLine(@"</tr>");
            bodyBuilder.AppendLine(@"</table>");
            bodyBuilder.AppendLine(@"</td>");
            bodyBuilder.AppendLine(@"</tr><!-- end tr -->");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<td valign=""middle"" class=""hero bg_white"" style=""padding: 2em 0 2em 0;"">");
            bodyBuilder.AppendLine(@"<table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<td style=""padding: 0 2.5em; text-align: left;"">");
            bodyBuilder.AppendLine(@"<div class=""text"">");
            bodyBuilder.AppendLine(@"<h3>We are delighted to inform you that your order has been successfully processed. Thank you for trusting and choosing our services.</h3>");
            bodyBuilder.AppendLine(@"</div>");
            bodyBuilder.AppendLine(@"</td>");
            bodyBuilder.AppendLine(@"</tr>");
            bodyBuilder.AppendLine(@"</table>");
            bodyBuilder.AppendLine(@"</td>");
            bodyBuilder.AppendLine(@"</tr><!-- end tr -->");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<table class=""bg_white"" role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<th style=""width: 20%; text-align: left; padding: 10px; border-bottom: 1px solid rgba(0, 0, 0, 0.05); color: #000;"">Information</th>");
            bodyBuilder.AppendLine(@"<th style=""width: 80%; text-align: left; padding: 10px; border-bottom: 1px solid rgba(0, 0, 0, 0.05); color: #000;"">Details</th>");
            bodyBuilder.AppendLine(@"</tr>");
            bodyBuilder.AppendLine(@"<table class=""bg_white"" role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<td style=""padding: 10px; border-bottom: 1px solid rgba(0, 0, 0, 0.05);"">Full Name:</td>");
            bodyBuilder.AppendLine(@"<td style=""padding: 10px; border-bottom: 1px solid rgba(0, 0, 0, 0.05);"">"+nameCus + @"</td>");
            bodyBuilder.AppendLine(@"</tr>");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<td style=""padding: 10px; border-bottom: 1px solid rgba(0, 0, 0, 0.05);"">Address:</td>");
            bodyBuilder.AppendLine(@"<td style=""padding: 10px; border-bottom: 1px solid rgba(0, 0, 0, 0.05);"">" + AddressCus + @"</td>");
            bodyBuilder.AppendLine(@"</tr>");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<td style=""padding: 10px; border-bottom: 1px solid rgba(0, 0, 0, 0.05);"">Phone Number:</td>");
            bodyBuilder.AppendLine(@"<td style=""padding: 10px; border-bottom: 1px solid rgba(0, 0, 0, 0.05);"">" + phoneCus + @"</td>");
            bodyBuilder.AppendLine(@"</tr>");
            bodyBuilder.AppendLine(@"<tr>");
            bodyBuilder.AppendLine(@"<td style=""padding: 10px;"">Email:</td>");
            bodyBuilder.AppendLine(@"<td style=""padding: 10px;"">" + email + @"</td>");
            bodyBuilder.AppendLine(@"</tr>");
            bodyBuilder.AppendLine(@"</table>");
            bodyBuilder.AppendLine("<tr>");
            bodyBuilder.AppendLine(@"<table class=""bg_white"" role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">");
            bodyBuilder.AppendLine(@"<tr style=""border-bottom: 1px solid rgba(0,0,0,.05);"">");
            bodyBuilder.AppendLine(@"<th width=""80%"" style=""text-align:left; padding: 0 2.5em; color: #000; padding-bottom: 20px"">Item</th>");
            bodyBuilder.AppendLine(@"<th width=""20%"" style=""text-align:right; padding: 0 2.5em; color: #000; padding-bottom: 20px"">Price</th>");
            bodyBuilder.AppendLine(@"</tr>");
            var httpContext = _httpContextAccessor.HttpContext;
            int orderCus = httpContext.Session.GetInt32("orderCus") ?? 0;
            //var orderList = GetOrderDetails(orderCus);
            var orderList = (from od in _context.OrderDetails
                             join o in _context.Orders on od.OrderId equals o.OrderId
                             join s in _context.Stocks on od.StockId equals s.StockId
                             join p in _context.Products on s.ProductId equals p.Pid
                             join i in _context.Images on p.Pid equals i.Pid
                             where i.ImageUrl.Contains("chinh") && o.OrderId == orderCus
                             select new
                             {
                                 OrderId = o.OrderId,
                                 Price = od.Price,
                                 Quantity = od.Quantity,
                                 ProductName = p.ProducctName,
                                 ImageUrl = i.ImageUrl
                             }).ToList();
            foreach (var item in orderList)
            {
                bodyBuilder.AppendLine("<tr style=\"border-bottom: 1px solid rgba(0,0,0,.05);\">");
                bodyBuilder.AppendLine("<td valign=\"middle\" width=\"80%\" style=\"text-align:left; padding: 0 2.5em;\">");
                bodyBuilder.AppendLine("<div class=\"product-entry\">");
                bodyBuilder.AppendLine(@"<img src=""" + item.ImageUrl + @""" alt="""" style=""width: 100px; max-width: 600px; height: auto; margin-bottom: 20px; display: block;"">");
                bodyBuilder.AppendLine("<div class=\"text\">");
                bodyBuilder.AppendLine($"<h3>{item.ProductName}</h3>");
                bodyBuilder.AppendLine($"<h3>Quantity:<span>{item.Quantity}</span></h3>");
                bodyBuilder.AppendLine("</div>");
                bodyBuilder.AppendLine("</div>");
                bodyBuilder.AppendLine("</td>");
                bodyBuilder.AppendLine("<td valign=\"middle\" width=\"20%\" style=\"text-align:left; padding: 0 2.5em;\">");
                bodyBuilder.AppendLine($"<span class=\"price\" style=\"color: #000; font-size: 20px;\">{item.Price}</span>");
                bodyBuilder.AppendLine("</td>");
                bodyBuilder.AppendLine("</tr>");
            }

            bodyBuilder.AppendLine("<tr>");
            bodyBuilder.AppendLine("<td valign=\"middle\" style=\"text-align:left; padding: 1em 2.5em;\">");
            bodyBuilder.AppendLine("<p>Glasses Shop</p>");
            bodyBuilder.AppendLine("</td>");
            bodyBuilder.AppendLine("</tr>");
            bodyBuilder.AppendLine("</table>");
            bodyBuilder.AppendLine("</tr>");
            bodyBuilder.AppendLine("</body>");
            bodyBuilder.AppendLine("</html>");
            bodyBuilder.AppendLine(@"</center>");
            return bodyBuilder.ToString();
        }
    }
}