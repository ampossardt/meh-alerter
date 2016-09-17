using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MehNotifications.Data
{
    public class Notification
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public bool SoldOutSent { get; set; }
        public bool AllSoldOutSent { get; set; }
        public bool SendNotification { get; set; }
    }
}