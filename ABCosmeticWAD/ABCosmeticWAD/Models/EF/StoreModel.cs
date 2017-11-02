using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class StoreModel
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreEmail { get; set; }
        public string StorePhone { get; set; }
    }
}