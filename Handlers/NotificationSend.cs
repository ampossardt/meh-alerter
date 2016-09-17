using System;
using System.Web;
using MehNotifications.Helpers;
using MehNotifications.Data;
using System.Text;

namespace MehNotifications.Handlers
{
    public class NotificationSend : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var deal = new APIHelper().DealsInfo;
            var notifications = DBHelper.GetUsers();

            if (deal == null)
            {
                return;
            }

            foreach (var notification in notifications)
            {
                var emailType = MailHelper.GetEmailType(deal, notification);

                MailArgs args = new MailArgs()
                {
                    Email = notification.Email,
                    ID = notification.ID
                };

                if (emailType == Alert.AlertLevel.OnSale)
                {
                    DBHelper.ExecuteCommand("dbo.ResetNotifications", args.ID);
                }

                if (!notification.SendNotification)
                {
                    continue;
                }
                else
                {
                    switch (emailType)
                    {
                        case Alert.AlertLevel.OnSale:
                        case Alert.AlertLevel.OnSaleEastCoast:
                            MailHelper.SendEmail(MailHelper.BuildEmail(deal, emailType, args), notification.Email);
                            break;

                        case Alert.AlertLevel.SoldOut:
                            DBHelper.ExecuteCommand("dbo.SetSoldOutSent", args.ID);
                            MailHelper.SendEmail(MailHelper.BuildEmail(deal, emailType, args), notification.Email);
                            break;

                        case Alert.AlertLevel.SoldOutEastCoast:
                            DBHelper.ExecuteCommand("dbo.SetAllSoldOutSent", args.ID);
                            MailHelper.SendEmail(MailHelper.BuildEmail(deal, emailType, args), notification.Email);
                            break;
                    }
                }

                StringBuilder response = new StringBuilder();

                response.AppendFormat("Task run successfully.{0}", Environment.NewLine);
                response.AppendFormat("User Info: Email - {0} ; ID - {1}{2}", args.Email, args.ID, Environment.NewLine);
                response.AppendFormat("Notification type: {0}{1}", emailType.ToString(), Environment.NewLine);

                var currentTime = DateTime.Now;

                if (HttpContext.Current.Request.Url.Host.Contains("andrewpossardt"))
                {
                    currentTime = currentTime.AddHours(3);
                }

                response.AppendFormat("Date Run: {0}{1}", currentTime.ToString(), Environment.NewLine);

                context.Response.Write(response.ToString());
            }
        }
    }
}
