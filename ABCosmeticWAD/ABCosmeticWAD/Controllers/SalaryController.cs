using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ABCosmeticWAD.Models;
using ABCosmeticWAD.Models.EF;

namespace ABCosmeticWAD.Controllers
{
    public class SalaryController : ApiController
    {
        ABCosmeticEntities db = new ABCosmeticEntities();
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult ret = null;
            ret = Ok(GetReportOnSalary());
            return ret;
        }

        [HttpGet()]
        public IHttpActionResult Get(int year)
        {
            IHttpActionResult ret = null;
            ret = Ok(GetByYear(year));
            return ret;
        }

        [HttpGet()]
        public IHttpActionResult Get(DateTime month)
        {
            IHttpActionResult ret = null;
            ret = Ok(GetByMonth(month));
            return ret;
        }

        [HttpGet()]
        public IHttpActionResult Get(DateTime from, DateTime to)
        {
            IHttpActionResult ret = null;
            ret = Ok(GetByDate(from, to));
            return ret;
        }

        private List<Salary> GetByDate(DateTime from, DateTime to)
        {
            List<Salary> list = new List<Salary>();
            try
            {
                db.Database.Connection.Open();
                List<Staff> listStaff = db.Staffs.Where(s => s.GroupID != "ADMIN").ToList();
                foreach (Staff staff in listStaff)
                {
                    Salary salary = new Salary();
                    salary.StaffID = staff.StaffID;
                    salary.StaffName = staff.StaffName;
                    salary.BaseSalary = staff.BaseSalary;
                    salary.StoreName = staff.Store.StoreName;
                    DateTime toDate = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);
                    List<Order> listOrder = db.Orders.Where(o => (o.StaffID == staff.StaffID) && (o.CreatedDate >= from) && (o.CreatedDate <= toDate)).ToList();
                    foreach (Order ord in listOrder)
                    {
                        List<OrderProduct> productList = ord.OrderProducts.ToList();
                        foreach (OrderProduct op in productList)
                        {
                            int? quant = op.Quantity;
                            double? price = op.Product.UnitPrice;
                            double? bonus = (quant * price) * 0.02;
                            if (bonus == null)
                            {
                                salary.Bonus = 0;
                            }
                            else
                            {
                                salary.Bonus = bonus;
                            }
                        }
                    }
                    list.Add(salary);
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return list;
        }

        private List<Salary> GetByMonth(DateTime month)
        {
            int date;
            DateTime to;
            DateTime from = new DateTime(month.Year, month.Month, 01, 0, 0, 0);
            if ((month.Month == 2) && (month.Year % 4 == 0))
            {
                date = 29;
                to = new DateTime(month.Year, month.Month, date, 23, 59, 59);
            }
            else if ((month.Month == 2) && (month.Year % 4 != 0))
            {
                date = 28;
                to = new DateTime(month.Year, month.Month, date, 23, 59, 59);
            }
            else if (month.Month == 1 || month.Month == 3 || month.Month == 5 || month.Month == 7 || month.Month == 8 || month.Month == 10 || month.Month == 12)
            {
                date = 31;
                to = new DateTime(month.Year, month.Month, date, 23, 59, 59);
            }
            else
            {
                date = 30;
                to = new DateTime(month.Year, month.Month, date, 23, 59, 59);
            }
            List<Salary> list = new List<Salary>();
            try
            {
                db.Database.Connection.Open();
                List<Staff> listStaff = db.Staffs.Where(s => s.GroupID != "ADMIN").ToList();
                foreach (Staff staff in listStaff)
                {
                    Salary salary = new Salary();
                    salary.StaffID = staff.StaffID;
                    salary.StaffName = staff.StaffName;
                    salary.BaseSalary = staff.BaseSalary;
                    salary.StoreName = staff.Store.StoreName;
                    List<Order> listOrder = db.Orders.Where(o => (o.StaffID == staff.StaffID) && (o.CreatedDate >= from) && (o.CreatedDate <= to)).ToList();
                    foreach (Order ord in listOrder)
                    {
                        List<OrderProduct> productList = ord.OrderProducts.ToList();
                        foreach (OrderProduct op in productList)
                        {
                            int? quant = op.Quantity;
                            double? price = op.Product.UnitPrice;
                            double? bonus = (quant * price) * 0.02;
                            if (bonus == null)
                            {
                                salary.Bonus = 0;
                            }
                            else
                            {
                                salary.Bonus = bonus;
                            }
                        }
                    }
                    list.Add(salary);
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return list;
        }

        private List<Salary> GetByYear(int year)
        {
            List<Salary> list = new List<Salary>();
            try
            {
                db.Database.Connection.Open();
                List<Staff> listStaff = db.Staffs.Where(s => s.GroupID != "ADMIN").ToList();
                foreach (Staff staff in listStaff)
                {
                    Salary salary = new Salary();
                    salary.StaffID = staff.StaffID;
                    salary.StaffName = staff.StaffName;
                    salary.BaseSalary = staff.BaseSalary;
                    salary.StoreName = staff.Store.StoreName;
                    List<Order> listOrder = db.Orders.Where(o => (o.StaffID == staff.StaffID) && (o.CreatedDate.Value.Year == year)).ToList();
                    foreach (Order ord in listOrder)
                    {
                        List<OrderProduct> productList = ord.OrderProducts.ToList();
                        foreach (OrderProduct op in productList)
                        {
                            int? quant = op.Quantity;
                            double? price = op.Product.UnitPrice;
                            double? bonus = (quant * price) * 0.02;
                            if (bonus == null)
                            {
                                salary.Bonus = 0;
                            }
                            else
                            {
                                salary.Bonus = bonus;
                            }
                        }
                    }
                    list.Add(salary);
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return list;
        }

        private List<Salary> GetReportOnSalary()
        {
            List<Salary> list = new List<Salary>();
            try
            {
                db.Database.Connection.Open();
                List<Staff> listStaff = db.Staffs.Where(s => s.GroupID != "ADMIN").ToList();
                foreach (Staff staff in listStaff)
                {
                    Salary salary = new Salary();
                    salary.StaffID = staff.StaffID;
                    salary.StaffName = staff.StaffName;
                    salary.BaseSalary = staff.BaseSalary;
                    salary.StoreName = staff.Store.StoreName;
                    List<Order> listOrder = db.Orders.Where(o => o.StaffID == staff.StaffID).ToList();
                    foreach (Order ord in listOrder)
                    {
                        List<OrderProduct> productList = ord.OrderProducts.ToList();
                        foreach (OrderProduct op in productList)
                        {
                            int? quant = op.Quantity;
                            double? price = op.Product.UnitPrice;
                            double? bonus = (quant * price) * 0.02;
                            if(bonus == null)
                            {
                                salary.Bonus = 0;
                            }
                            else
                            {
                                salary.Bonus = bonus;
                            }
                        }
                    }
                    list.Add(salary);
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return list;
        }
    }
}
