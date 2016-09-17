using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MehNotifications.Data
{
    public class Alert
    {
        public enum AlertLevel
        {
            OnSale = 0,
            OnSaleEastCoast = 1,
            SoldOut = 2,
            SoldOutEastCoast = 3,
            None = 4
        }
    }
}
