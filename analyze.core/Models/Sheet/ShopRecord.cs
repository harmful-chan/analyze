using analyze.core.Models.Daily;
using Esprima.Ast;
using IPinfo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{
    public class ShopRecord : IComparable<ShopRecord>
    {
        public Shop Shop { get; set; }
        public string RecordDir { get; set; }
        public DateTime CollectDate { get; set; }
        public IList<ShopOrder> ShopOrderList { get; set; }
        public IList<ShopLend> ShopLendList { get; set; }
        public IList<ShopRefund> ShopRefundList { get; set; }

        public IList<DailyDetail> DailyDetailsList { get; set; }

        public int CompareTo([AllowNull] ShopRecord other)

        {
            int index = Shop.CompanyNumber.CompareTo(other.Shop.CompanyNumber);
            //if (index == 0)
            //{
            //    index = CompanyNumber.CompareTo(other.CompanyNumber);
            //    if (index == 0)
            //    {
            //        index = Company.CompareTo(other.Company);
            //        if (index == 0)
            //        {
            //            return Nick.CompareTo(other.Nick);
            //        }
            //    }
            //}

            return index;
        }
    }
}