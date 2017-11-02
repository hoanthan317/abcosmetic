using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABCosmeticWAD.Models.EF;

namespace ABCosmeticWAD.Models.EF
{
    public class StoreRevenue
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public List<OrderPrice> Order { get; set; }
    }
}