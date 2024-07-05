using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Daily
{
    public class StoreOverview
    {
        public int ID { get; set; }
        public string Company { get; set; }
        public string CN { get; set; }
        public string Opera { get; set; }
        public int UP { get; set; }
        public int Check { get; set; }
        public int Down { get; set; }
        public double IM24 { get; set; }
        public double Good { get; set; }
        public double Dispute { get; set; }
        public double Wrong { get; set; }
        public string DisputeLine { get; set; }
        public int F30 { get; set; }
        public int D30 { get; set; }
        public int Exp30 { get; set; }
        public int Fin { get; set; }
        public int Dis { get; set; }
        public int Close { get; set; }
        public int Talk { get; set; }
        public int Palt { get; set; }
        public int All { get; set; }
        public string ReadyLine { get; set; }
        public int New { get; set; }
        public int Ready { get; set; }
        public int Wait { get; set; }
        public Funds Funds { get; set; }
    }
}
