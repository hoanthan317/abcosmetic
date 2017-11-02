using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABCosmeticWAD.Models.EF;
using ABCosmeticWAD.Common;
using ABCosmeticWAD.Models;
using System.Web.Security;

namespace ABCosmeticWAD.Controllers
{
    public class LoginController : Controller
    {
        ABCosmeticEntities db = new ABCosmeticEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Database.Connection.Open();
                    string hashedPass = MD5Encryptor.MD5Hash(user.Password);
                    Staff staff = db.Staffs.SingleOrDefault(s => s.StaffID == user.StaffID && s.Password == hashedPass);
                    if (staff != null)
                    {
                        if(staff.GroupID == "MANAGER" || staff.GroupID == "STAFF" || staff.GroupID == "ADMIN")
                        {
                            Session.RemoveAll();
                            Session.Add(CommonConstants.USER_SESSION, staff);
                            List<string> permission = new List<string>();
                            var group = staff.UserGroup;
                            if (staff.UserGroup != null)
                            {
                                foreach (UserAction action in group.UserActions)
                                {
                                    permission.Add(action.ActionID);
                                }
                                Session.Add(CommonConstants.PERMISSION_SESSION, permission);
                            }
                            Session.Timeout = 1400;
                            db.Database.Connection.Close();
                            if (staff.GroupID == "STAFF")
                            {
                                return RedirectToAction("AddOrder", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            db.Database.Connection.Close();
                            ModelState.AddModelError("", "You don't have permission to login as an Adminstrator");
                            return View("Login");
                        }
                    }
                    else
                    {
                        db.Database.Connection.Close();
                        ModelState.AddModelError("", "Wrong Staff ID or Password");
                        return View("Login");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Something went wrong. Please come back later or call +841637971434 for help");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login fail!");
            }
            return View("Login");
        }

    }
}