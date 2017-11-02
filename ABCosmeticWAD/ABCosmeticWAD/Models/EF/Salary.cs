using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class Salary
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public double? BaseSalary { get; set; }
        public double? Bonus { get; set; }
        public string StoreName { get; set; }
    }
}