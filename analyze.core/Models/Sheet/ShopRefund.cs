using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{
    public class ShopRefund
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; } 
        public string SUK { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public double Turnover { get; set; }
        public double Refund { get; set; }
        public string Sources { get; set; }
        public double Fee { get; set; }
        public double Alliance { get; set; }
        public double Cashback { get; set; }
        public DateTime RefundTime { get; set; }
        public string RefundReason { get; set; }
        public string DebitOperation { get; set; }
        public double Deduction { get; set; }
        public string RefundOperation { get; set; }
        public string RefundId { get; set; }
        public string Trade { get; set; }
        public string FileName { get; set; }
    }
}
