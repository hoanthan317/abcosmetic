using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class StaffModel
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string GroupName { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreName { get; set; }
        public double? BaseSalary { get; set; }
    }
}