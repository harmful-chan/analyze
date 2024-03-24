using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace analyze.core.Models.Daily
{
    public class DailyDetail : IComparable<DailyDetail>
    {

        public DateTime CollectionDate { get; set; }
        public string CompanyNumber { get; set; }
        public string Company { get; set; }
        public string CN { get; set; }
        public string Nick { get; set; }
        public string Operator { get; set; }

        public double Promotion { get; set; }

        public double Consume { get; set; }

        public int InSrockNumber { get; set; }
        public int ReviewNumber { get; set; }
        public int RemovedNumber { get; set; }
        public double IM24 { get; set; }
        public double WrongGoods { get; set; }
        public double NotSell { get; set; }
        public double Dispute { get; set; }
        public double GoodReviews { get; set; }
        public double Collect72 { get; set; }
        public double Lend { get; set; }
        public double Freeze { get; set; }
        public double OnWay { get; set; }
        public double Arrears { get; set; }

        public Withdraw[] Withdraws { get; set; }
        public NotShip[] NotShips { get; set; }
        public OrderDetail[] OrderDetails { get; set; }
        public Dispute[] DisputeOrders { get; set; }

        public OnWayOrder[] OnWayOrders { get; set; }


        public PromotionDetail[] PromotionDetails { get; set; }

        public int CompareTo([AllowNull] DailyDetail other)
            
        {
            int index = CollectionDate.CompareTo(other.CollectionDate);
            if (index == 0)
            {
                index = CompanyNumber.CompareTo(other.CompanyNumber);
                if (index == 0)
                {
                    index = Company.CompareTo(other.Company);
                    if (index == 0)
                    {
                        return Nick.CompareTo(other.Nick);
                    }
                }
            }

            return index;
        }
    }
}
