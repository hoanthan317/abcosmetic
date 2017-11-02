using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class ProductModel
    {
        [DisplayName(displayName: "Product ID:")]
        public int ProductID { get; set; }

        [DisplayName(displayName: "Category:")]
        public string ProductCate { get; set; }

        [DisplayName(displayName: "Brand:")]
        public string Brand { get; set; }

        [DisplayName(displayName: "Model:")]
        public string Model { get; set; }

        [DisplayName(displayName: "Product Name:")]
        public string ProductName { get; set; }

        public string Description { get; set; }
        public string Detail { get; set; }

        [DisplayName(displayName: "Instock quantity:")]
        public Nullable<int> UnitInStock { get; set; }

        [DisplayName(displayName: "Unit Price:")]
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<int> Viewed { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}