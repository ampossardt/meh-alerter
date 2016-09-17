using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MehNotifications.Data
{
    public class Item
    {
        public string Price { get; set; }
        public string Condition { get; set; }
        public List<string> Attributes { get; set; }

        public override string ToString()
        {
            return string.Format("Price: {0}, Condition: {1}, Attributes: {2}", Price, Condition, string.Join(",", Attributes));
        }
    }
}
