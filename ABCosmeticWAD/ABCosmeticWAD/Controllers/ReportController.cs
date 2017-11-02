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
    public class ReportController : ApiController
    {
        ABCosmeticEntities db = new ABCosmeticEntities();
        [HttpGet()]
        public IHttpActionResult Get(int year, string store)
        {
            IHttpActionResult ret = null;
            ret = Ok(GetByYear(year, store));
            return ret;
        }

        [HttpGet()]
        public IHttpActionResult Get(DateTime month, string store)
        {
            IHttpActionResult ret = null;
            ret = Ok(GetByMonth(month, store));
            return ret;
        }

        [HttpGet()]
        public IHttpActionResult Get(DateTime from, DateTime to, string store)
        {
            IHttpActionResult ret = null;
            ret = Ok(GetByDate(from, to, store));
            return ret;
        }


        private List<Revenue> GetByDate(DateTime from, DateTime to, string store)
        {
            List<Revenue> list = new List<Revenue>();
            try
            {
                db.Database.Connection.Open();
                DateTime toDate = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);
                List<Order> order = db.Orders.Where(o => (o.CreatedDate <= toDate) && (o.CreatedDate >= from)).ToList();
                for (var i = from; i < toDate; i = i.AddDays(1))
                {
                    Revenue re = new Revenue();
                    re.Date = i;
                    re.SalesFigure = 0;
                    List<Order> dateOrder = order.Where(m => (m.CreatedDate.Value.Year == i.Year) && (m.CreatedDate.Value.Month == i.Month) && (m.CreatedDate.Value.Day == i.Day)).ToList();
                    foreach (Order od in dateOrder)
                    {
                        List<OrderProduct> op = od.OrderProducts.ToList();
                        foreach (OrderProduct ordpro in op)
                        {
                            re.SalesFigure += (ordpro.Price * ordpro.Quantity);
                        }
                    }
                    list.Add(re);
                }
                list.OrderBy(m => m.Date);
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

        private List<Revenue> GetByMonth(DateTime month, string store)
        {
            List<Revenue> list = new List<Revenue>();
            int date;
            DateTime to;
            DateTime from = new DateTime(month.Year, month.Month, 01);
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
            try
            {
                db.Database.Connection.Open();
                List<Order> order = new List<Order>();
                if (store != "null")
                {
                    order = db.Orders.Where(o => (o.CreatedDate <= to) && (o.CreatedDate >= from) && (o.StoreName == store)).ToList();
                }
                else
                {
                    order = db.Orders.Where(o => (o.CreatedDate <= to) && (o.CreatedDate >= from)).ToList();
                }
                for (int i = 1; i <= date; i++)
                {
                    Revenue re = new Revenue();
                    re.Time = i;
                    re.SalesFigure = 0;
                    List<Order> dateOrder = order.Where(m => m.CreatedDate.Value.Day == i).ToList();
                    foreach (Order od in dateOrder)
                    {
                        List<OrderProduct> op = od.OrderProducts.ToList();
                        foreach (OrderProduct ordpro in op)
                        {
                            re.SalesFigure += (ordpro.Price * ordpro.Quantity);
                        }
                    }
                    list.Add(re);
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

        private List<Revenue> GetByYear(int year, string store)
        {
            List<Revenue> list = new List<Revenue>();
            try
            {
                db.Database.Connection.Open();
                List<Order> order = new List<Order>();
                if (store != "null")
                {
                    order = db.Orders.Where(m => (m.CreatedDate.Value.Year == year) && (m.StoreName == store)).ToList();
                }
                else
                {
                    order = db.Orders.Where(m => m.CreatedDate.Value.Year == year).ToList();
                }
                for (int month = 1; month <= 12; month++)
                {
                    Revenue re = new Revenue();
                    re.Time = month;
                    re.SalesFigure = 0;
                    List<Order> monthOrder = order.Where(m => m.CreatedDate.Value.Month == month).ToList();
                    foreach (Order od in monthOrder)
                    {
                        List<OrderProduct> op = od.OrderProducts.ToList();
                        foreach (OrderProduct ordpro in op)
                        {
                            re.SalesFigure += (ordpro.Price * ordpro.Quantity);
                        }
                    }
                    list.Add(re);
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
