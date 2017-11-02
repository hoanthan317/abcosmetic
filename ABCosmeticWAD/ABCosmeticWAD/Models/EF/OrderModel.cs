using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public string StaffName { get; set; }
        public string Store { get; set; }
        public string ShippingAddress { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string ContactPhone { get; set; }
        public string PaymentMethodID { get; set; }
    }
}