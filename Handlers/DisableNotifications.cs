using System;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using MehNotifications.Helpers;

namespace MehNotifications.Handlers
{
    public class DisableNotifications : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // disable notifications for meh status changes
            var queryString = context.Request.QueryString;
            int id = 0;

            if(int.TryParse(queryString["id"], out id)) 
            {
                try
                {
                    DBHelper.ExecuteCommand("dbo.DisableNotifications", id);
                }
                catch(Exception e) 
                {
                    context.Response.Write("There was an error disabling notifications. Please try again.");
                }

                context.Response.Write("Notifications successfully disabled for today's deal.");
            }
        }
    }
}
