using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABCosmeticWAD.Models.EF
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter Staff ID")]
        [DisplayName(displayName: "Staff ID")]
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }

        [DisplayName(displayName: "Remember me")]
        public bool Remember { get; set; }
    }
}