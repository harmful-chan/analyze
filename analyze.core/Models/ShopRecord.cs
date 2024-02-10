using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Models
{
    public class ShopRecord
    {
        public Shop Shop { get; set; }
        public IList<ShopOrder> ShopOrderList { get; set; }
        public IList<ShopLend> ShopLendList { get; set; }
        public IList<ShopRefund> ShopRefundList { get; set; }
    }
}
