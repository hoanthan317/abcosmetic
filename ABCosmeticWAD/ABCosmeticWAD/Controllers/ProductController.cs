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
    public class ProductController : ApiController
    {
        ABCosmeticEntities db = new ABCosmeticEntities();

        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult ret = null;
            List<ProductModel> list = GetAllProduct();
            ret = Ok(list);
            return ret;
        }

        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult ret = null;
            ProductModel product = GetById(id);
            ret = Ok(product);
            return ret;
        }

        private ProductModel GetById(int id)
        {
            ProductModel product;
            try
            {
                db.Database.Connection.Open();
                Product pro = db.Products.SingleOrDefault(m => m.ProductID == id);
                string cate = null;
                string brand = null;
                if (pro.ProductCateID != null)
                {
                    cate = pro.ProductCategory.CategoriesName;
                }
                if (pro.BrandID != null)
                {
                    brand = pro.Brand.BrandName;
                }
                product = new ProductModel()
                {
                    ProductID = pro.ProductID,
                    ProductName = pro.ProductName,
                    Brand = brand,
                    ProductCate = cate,
                    CreatedDate = pro.CreatedDate,
                    Description = pro.Description,
                    UnitPrice = pro.UnitPrice,
                    Detail = pro.Detail,
                    Model = pro.Model,
                    ModifiedDate = pro.ModifiedDate,
                    Rate = pro.Rate,
                    UnitInStock = pro.UnitInStock,
                    Viewed = pro.Viewed
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
            return product;
        }

        private List<ProductModel> GetAllProduct()
        {
            List<ProductModel> list = new List<ProductModel>();
            try
            {
                db.Database.Connection.Open();
                List<Product> proList = db.Products.ToList();
                foreach (Product pro in proList)
                {
                    string cate = null;
                    string brand = null;
                    if (pro.ProductCateID != null)
                    {
                        cate = pro.ProductCategory.CategoriesName;
                    }
                    if (pro.BrandID != null)
                    {
                        brand = pro.Brand.BrandName;
                    }
                    list.Add(new ProductModel
                    {
                        ProductID = pro.ProductID,
                        ProductName = pro.ProductName,
                        Brand = brand,
                        ProductCate = cate,
                        CreatedDate = pro.CreatedDate,
                        Description = pro.Description,
                        UnitPrice = pro.UnitPrice,
                        Detail = pro.Detail,
                        Model = pro.Model,
                        ModifiedDate = pro.ModifiedDate,
                        Rate = pro.Rate,
                        UnitInStock = pro.UnitInStock,
                        Viewed = pro.Viewed
                    });
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
