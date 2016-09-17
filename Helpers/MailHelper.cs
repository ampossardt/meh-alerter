using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using MehNotifications.Data;
using System.Configuration;

namespace MehNotifications.Helpers
{
    public class MailHelper
    {

        public MailHelper()
        {
        }

        public static void SendEmail(string body, string recipient)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "relay-hosting.secureserver.net";
            client.Port = 25;

            MailMessage message = new MailMessage("notifications@andrewpossardt.com", recipient, "Meh Daily Deal Status Change", body);
            message.IsBodyHtml = true;
            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                
            }
        }

        public static string BuildEmail(Deal dealInfo, Alert.AlertLevel alertLevel, MailArgs args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(BuildEmailHeader(dealInfo, alertLevel));
            sb.Append(BuildEmailProductInfo(dealInfo, args));

            return sb.ToString();
        }

        /// <summary>
        /// Builds the product section of the email template
        /// </summary>
        /// <param name="dealInfo"></param>
        /// <returns></returns>
        private static string BuildEmailProductInfo(Deal dealInfo, MailArgs args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<hr style=\"margin-top: 30px; margin-bottom: 30px;\"/>");
            sb.AppendFormat("<h2>{0}</h2>", dealInfo.Title);
            sb.AppendFormat(dealInfo.Description.Replace("\r\n", "<br/>"));
            sb.Append("<br/>");
            sb.AppendFormat("<p><strong>Condition:</strong> {0}</p>", dealInfo.Items[0].Condition);
            sb.AppendFormat("<p><strong>Price:</strong> ${0}</p>", dealInfo.Items[0].Price);
            sb.Append("<table><tr><th style=\"padding: 15px; background-color: #111; color: #fff;\">Options</th></tr>");
            foreach (Item item in dealInfo.Items)
            {
                var attributes = string.Join("<br/>", item.Attributes);
                sb.AppendFormat("<tr><td style=\"padding: 15px; border: 1px solid #111;\">{1}</td></tr>",
                                item.Condition, attributes);
            }
            sb.AppendFormat("</table><p><a style=\"font-size:18px\" target=\"_blank\" href=\"{0}\">See full listing on meh.com</a></p><br/>", dealInfo.Url);
            sb.AppendFormat("<table><tr><td style=\"background-color:#000;width:100px;height:100px;border-radius:100px;text-align:center;\"><a style=\"font-size: 19px; text-decoration: none;color:#fff;\" href=\"DisableNotifications.ashx?id=" + args.ID + "\">meh.</a></td></tr></table>");

            return sb.ToString();
        }

        private static string BuildEmailHeader(Deal dealInfo, Alert.AlertLevel alertLevel)
        {
            StringBuilder sb = new StringBuilder();

            switch (alertLevel)
            {
                case Alert.AlertLevel.OnSale:
                    sb.Append("<p>Yo, the new deal of the day is up on meh.com. Full info on what's for sale below.</p>");
                    break;
                case Alert.AlertLevel.SoldOut:
                    sb.Append("<p>Damn, todays daily deal sold out. No worries, this is just the first stock. A bunch more will go on sale at 8:00 EST.</p>");
                    break;
                case Alert.AlertLevel.OnSaleEastCoast:
                    sb.Append("<p>Alright, todays daily deal is restocked! If you missed your chance before, go check it out.</p>");
                    break;
                case Alert.AlertLevel.SoldOutEastCoast:
                    sb.Append("<p>Hope you didn't want any of the daily deal, cause it's completely sold out now.</p>");
                    break;
            }

            return sb.ToString();
        }

        public static Alert.AlertLevel GetEmailType(Deal deal, Notification notification)
        {
            var currentTime = DateTime.Now;

            if (HttpContext.Current.Request.Url.Host.Contains("andrewpossardt"))
            {
                currentTime = currentTime.AddHours(3);
            }

            if (currentTime > DateTime.Today.AddMinutes(1) &&
                       currentTime < DateTime.Today.AddMinutes(9))
            {
                return Alert.AlertLevel.OnSale;
            }
            else if (!notification.SoldOutSent && currentTime < DateTime.Today.AddHours(8) && deal.Launches[0] < currentTime)
            {
                return Alert.AlertLevel.SoldOut;
            }
            else if (currentTime > DateTime.Today.AddHours(8).AddMinutes(1) &&
                    currentTime < DateTime.Today.AddHours(8).AddMinutes(9) &&
                    deal.Launches[0] < currentTime)
            {
                return Alert.AlertLevel.OnSaleEastCoast;
            }
            else if (!notification.AllSoldOutSent && currentTime > DateTime.Today.AddHours(8).AddMinutes(10) && deal.Launches[1] < currentTime)
            {
                return Alert.AlertLevel.SoldOutEastCoast;
            }

            return Alert.AlertLevel.None;
        }
    }


}
