using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ABCosmeticWAD.Models.EF;
using ABCosmeticWAD.Models;

namespace ABCosmeticWAD.Controllers
{
    public class PaymentMethodController : ApiController
    {
        ABCosmeticEntities db = new ABCosmeticEntities();
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult ret = null;
            ret = Ok(GetAllPaymentMethod());
            return ret;
        }

        private List<Payment> GetAllPaymentMethod()
        {
            List<Payment> list = new List<Payment>();
            try
            {
                db.Database.Connection.Open();
                foreach (PaymentMethod p in db.PaymentMethods.ToList())
                {
                    list.Add(new Payment { PaymentMethodID = p.PaymentMethodID, PaymentMethodName = p.PaymentMethodName });
                };
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
