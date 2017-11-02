using ABCosmeticWAD.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ABCosmeticWAD.Models;
using ABCosmeticWAD.Common;

namespace ABCosmeticWAD.Controllers
{
    public class OrderController : ApiController
    {
        ABCosmeticEntities db = new ABCosmeticEntities();

        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult ret = null;
            List<OrderModel> list = new List<OrderModel>();
            list = GetAllOrders();
            ret = Ok(list);
            return ret;
        }

        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult ret = null;
            OrderModel order = GetOrderById(id);
            ret = Ok(order);
            return ret;
        }

        [HttpPost()]
        public IHttpActionResult Add(int id, Order order)
        {
            IHttpActionResult ret = null;
            var oid = CreateOrder(id, order);
            if (oid != null)
            {
                ret = Ok(oid);
            }
            else
            {
                ret = Ok(false);
            }
            return ret;
        }

        [HttpPut()]
        public IHttpActionResult Update(int id, Order order)
        {
            IHttpActionResult ret = null;
            ret = Ok(UpdateOrder(id, order));
            return ret;
        }

        private bool UpdateOrder(int id, Order info)
        {
            try
            {
                db.Database.Connection.Open();
                Order order = db.Orders.SingleOrDefault(o => o.OrderID == id);
                if (order != null)
                {
                    order.CustomerName = info.CustomerName;
                    order.ContactPhone = info.ContactPhone;
                    order.Note = info.Note;
                    order.ShippingAddress = info.ShippingAddress;
                    order.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return true;
        }

        private int? CreateOrder(int id, Order order)
        {
            int? oid;
            try
            {
                db.Database.Connection.Open();
                string StoreName = db.Staffs.SingleOrDefault(m => m.StaffID == id).Store.StoreName;
                Order od = new Order() { StaffID = id, StoreName = StoreName, CustomerName = order.CustomerName, ContactPhone = order.ContactPhone, ShippingAddress = order.ShippingAddress, CreatedDate = DateTime.Now, PaymentMethodID = order.PaymentMethodID, Note = order.Note };
                db.Orders.Add(od);
                db.SaveChanges();
                oid = od.OrderID;

            }
            catch
            {
                return null;
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return oid;
        }

        private OrderModel GetOrderById(int id)
        {
            OrderModel order = new OrderModel();
            try
            {
                db.Database.Connection.Open();
                Order od = db.Orders.SingleOrDefault(o => o.OrderID == id);
                order.OrderID = od.OrderID;
                order.CustomerName = od.CustomerName;
                order.ContactPhone = od.ContactPhone;
                order.StaffName = od.Staff.StaffName;
                order.Store = od.Staff.Store.StoreName;
                order.ShippingAddress = od.ShippingAddress;
                order.Note = od.Note;
                order.OrderDate = od.OrderDate;
                order.ShipDate = od.ShipDate;
                order.CreatedDate = String.Format("{0:G}", od.CreatedDate);
                order.ModifiedDate = String.Format("{0:G}", od.ModifiedDate);
                order.PaymentMethodID = od.PaymentMethodID;
            }
            catch
            {
                return null;
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return order;
        }

        private List<OrderModel> GetAllOrders()
        {
            List<OrderModel> list = new List<OrderModel>();
            try
            {
                List<Order> ord = db.Orders.ToList();
                foreach (Order o in ord)
                {
                    string StaffName = null;
                    string StoreName = null;
                    string Status = null;
                    if (o.StatusID != null)
                    {
                        Status = db.OrderStatus.SingleOrDefault(s => s.StatusID == o.StatusID).StatusName;
                    }
                    if (o.StaffID != null)
                    {
                        StaffName = db.Staffs.SingleOrDefault(s => s.StaffID == o.StaffID).StaffName;
                    }
                    if (o.StaffID != null)
                    {
                        StoreName = db.Staffs.SingleOrDefault(s => s.StaffID == o.StaffID).Store.StoreName;
                    }
                    list.Add(new OrderModel { OrderID = o.OrderID, CustomerName = o.CustomerName, ContactPhone = o.ContactPhone, StaffName = StaffName, Store = StoreName, ShippingAddress = o.ShippingAddress, Note = o.Note, OrderDate = o.OrderDate, ModifiedDate = String.Format("{0:G}", o.ModifiedDate), CreatedDate = String.Format("{0:G}", o.CreatedDate), ShipDate = o.ShipDate, Status = Status, PaymentMethodID = o.PaymentMethodID });
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
