using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class OrderPrice
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}