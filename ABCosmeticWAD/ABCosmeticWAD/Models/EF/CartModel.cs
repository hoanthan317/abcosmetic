using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class CartModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public int UnitInStock { get; set; }
    }
}