using ABCosmeticWAD.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ABCosmeticWAD.Models;

namespace ABCosmeticWAD.Controllers
{
    public class OrderDetailController : ApiController
    {
        ABCosmeticEntities db = new ABCosmeticEntities();
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult ret = null;
            ret = Ok(GetProductByOrderId(id));
            return ret;
        }

        [HttpPost()]
        public IHttpActionResult Post(int id, List<CartModel> cart)
        {
            IHttpActionResult ret = null;
            ret = Ok(AddProduct(id, cart));
            return ret;
        }

        private bool AddProduct(int orderId, List<CartModel> cart)
        {
            try
            {
                db.Database.Connection.Open();
                foreach(CartModel c in cart)
                {
                    OrderProduct product = new OrderProduct();
                    product.OrderID = orderId;
                    product.ProductID = c.ProductID;
                    product.Quantity = c.Quantity;
                    product.Price = c.UnitPrice;
                    db.OrderProducts.Add(product);
                }
                db.SaveChanges();
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

        private List<CartModel> GetProductByOrderId(int id)
        {
            List<CartModel> list = new List<CartModel>();
            try
            {
                db.Database.Connection.Open();
                List<OrderProduct> product = db.OrderProducts.Where(o => o.OrderID == id).ToList();
                foreach (OrderProduct p in product)
                {
                    list.Add(new CartModel { ProductID = p.ProductID, ProductName = p.Product.ProductName, Quantity = p.Quantity, UnitPrice = p.Price });
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
