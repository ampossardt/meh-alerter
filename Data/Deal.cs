using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MehNotifications.Data
{
    public class Deal
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool InStock { get; set; }
        public string Url { get; set; }
        public List<Item> Items { get; set; }
        public List<DateTime> Launches { get; set; }
        public string Photo { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Title: {0}\n", Title);
            sb.AppendFormat("Description: {0}\n", Description);
            sb.AppendFormat("Url: {0}\n", Url);
            sb.AppendFormat("Item Options: {0}\n", string.Join(";", Items));
            sb.AppendFormat("Launches: {0}\n", string.Join(",", Launches));
            sb.AppendFormat("Photo Url: {0}\n", Photo);

            return sb.ToString();
        }
    }
}
