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
    public class StoreController : ApiController
    {
        ABCosmeticEntities db = new ABCosmeticEntities();
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult ret = null;
            ret = Ok(GetAll());
            return ret;
        }

        private List<StoreModel> GetAll()
        {
            List<StoreModel> list = new List<StoreModel>();
            try
            {
                db.Database.Connection.Open();
                List<Store> stores = db.Stores.ToList();
                foreach(Store s in stores)
                {
                    list.Add(new StoreModel { StoreID = s.StoreID, StoreName = s.StoreName, StoreAddress = s.StoreAddress, StoreEmail = s.StoreEmail, StorePhone = s.StorePhone });
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
