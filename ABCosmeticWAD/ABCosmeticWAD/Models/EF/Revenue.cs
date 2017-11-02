using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class Revenue
    {
        public int Time { get; set; }
        public DateTime Date { get; set; }
        public double? SalesFigure { get; set; }
    }
}