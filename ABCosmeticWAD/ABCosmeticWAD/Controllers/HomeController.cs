using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ABCosmeticWAD.Models.EF;
using ABCosmeticWAD.Common;
using Newtonsoft.Json;
using ABCosmeticWAD.Models;

namespace ABCosmeticWAD.Controllers
{
    public class HomeController : Controller
    {
        ABCosmeticEntities db = new ABCosmeticEntities();

        [HasCredential(Action = "VIEW_REPORT")]
        public ActionResult Index()
        {
            return View();
        }

        [HasCredential(Action = "VIEW_PRODUCT")]
        public ActionResult Product()
        {
            ViewBag.List = GetAllProduct();
            return View();
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }

        private List<ProductModel> GetAllProduct()
        {
            List<ProductModel> list = new List<ProductModel>();
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
            db.Database.Connection.Close();
            return list;
        }

        [HasCredential(Action = "DELETE_PRODUCT")]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                db.Database.Connection.Open();
                Product delPro = db.Products.SingleOrDefault(p => p.ProductID == id);
                if (delPro != null)
                {
                    db.Products.Remove(delPro);
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("Product");
                }
                else
                {
                    TempData["error"] = "The seleted product is no longer available";
                    db.Database.Connection.Close();
                    return RedirectToAction("Product");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong, cannot delete selected product. Please refresh the page or try again later";
                return RedirectToAction("Product");
            }
        }

        [HasCredential(Action = "VIEW_PRODUCT")]
        public ActionResult ProductDetail(int id)
        {
            try
            {
                db.Database.Connection.Open();
                Product pro = db.Products.SingleOrDefault(p => p.ProductID == id);
                if (pro != null)
                {
                    string brand = null;
                    string cate = null;
                    if (pro.ProductCateID != null)
                    {
                        cate = pro.ProductCategory.CategoriesName;
                    }
                    if (pro.BrandID != null)
                    {
                        brand = pro.Brand.BrandName;
                    }
                    ProductModel product = new ProductModel()
                    {
                        ProductID = pro.ProductID,
                        ProductName = pro.ProductName,
                        Brand = brand,
                        ProductCate = cate,
                        Description = pro.Description,
                        Detail = pro.Detail,
                        Model = pro.Model,
                        UnitInStock = pro.UnitInStock,
                        UnitPrice = pro.UnitPrice,
                        CreatedDate = pro.CreatedDate,
                        ModifiedDate = pro.ModifiedDate,
                        Rate = pro.Rate,
                        Viewed = pro.Viewed
                    };
                    db.Database.Connection.Close();
                    ViewBag.Cate = db.ProductCategories;
                    ViewBag.Brand = db.Brands;
                    ViewBag.Product = product;
                    return View();
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "Something went wrong. Cannot find selected product. Please try again later";
                    return RedirectToAction("Product", "Home");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. Cannot find selected product. Please try again later";
                return RedirectToAction("Product", "Home");
            }
        }

        [HasCredential(Action = "UPDATE_PRODUCT")]
        public ActionResult UpdateProduct(ProductModel value)
        {
            try
            {
                db.Database.Connection.Open();
                Product product = db.Products.SingleOrDefault(p => p.ProductID == value.ProductID);
                if (product != null)
                {
                    product.ProductID = value.ProductID;
                    product.ProductName = value.ProductName;
                    product.ProductCateID = db.ProductCategories.SingleOrDefault(c => c.CategoriesName == value.ProductCate).ProductCateID;
                    product.BrandID = db.Brands.SingleOrDefault(b => b.BrandName == value.Brand).BrandID;
                    product.Description = value.Description;
                    product.Detail = value.Detail;
                    product.Model = value.Model;
                    product.UnitInStock = value.UnitInStock;
                    product.UnitPrice = value.UnitPrice;
                    product.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("ProductDetail", "Home", new { id = product.ProductID });
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "Cannot update the product. Please try again later or contact your mananger";
                    return RedirectToAction("Product");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. Contact your manager or try again later";
                return RedirectToAction("Product");
            }
        }

        [HasCredential(Action = "ADD_PRODUCT")]
        public ActionResult CreateProduct()
        {
            db.Database.Connection.Open();
            ViewBag.Cate = db.ProductCategories;
            ViewBag.Brand = db.Brands;
            db.Database.Connection.Close();
            return View();
        }

        [HasCredential(Action = "ADD_PRODUCT")]
        [HttpPost]
        public ActionResult CreateProduct(Product value)
        {
            try
            {
                db.Database.Connection.Open();
                value.CreatedDate = DateTime.Now;
                db.Products.Add(value);
                db.SaveChanges();
                db.Database.Connection.Close();
                return RedirectToAction("ProductDetail", new { id = value.ProductID });
            }
            catch
            {
                TempData["error"] = "Something went wrong. Please contact your manager or try again later";
            }
            return RedirectToAction("Product");
        }

        [HasCredential(Action = "VIEW_MANAGER")]
        public ActionResult Manager()
        {
            ViewBag.List = GetAllManager();
            return View();
        }

        private List<StaffModel> GetAllManager()
        {
            db.Database.Connection.Open();
            List<StaffModel> mng = new List<StaffModel>();
            List<Staff> list = db.Staffs.Where(s => s.GroupID == "MANAGER").ToList();
            foreach (Staff m in list)
            {
                string GroupName = null;
                string StoreName = null;
                if (m.GroupID != null)
                {
                    GroupName = db.UserGroups.SingleOrDefault(g => g.GroupID == m.GroupID).GroupName;
                }
                if (m.StoreID != null)
                {
                    StoreName = db.Stores.SingleOrDefault(s => s.StoreID == m.StoreID).StoreName;
                }
                mng.Add(new StaffModel { StaffID = m.StaffID, StaffName = m.StaffName, Gender = m.Gender, Email = m.Email, PhoneNumber = m.PhoneNumber, Address = m.Address, DateOfBirth = m.DateOfBirth, GroupName = GroupName, StoreName = StoreName, BaseSalary = m.BaseSalary });
            }
            db.Database.Connection.Close();
            return mng;
        }

        [HasCredential(Action = "VIEW_MANAGER")]
        public ActionResult ManagerDetail(int id)
        {
            try
            {
                db.Database.Connection.Open();
                Staff staff = db.Staffs.SingleOrDefault(p => p.StaffID == id);
                if (staff != null)
                {
                    string group = null;
                    string store = null;
                    if (staff.GroupID != null)
                    {
                        group = staff.UserGroup.GroupName;
                    }
                    if (staff.StoreID != null)
                    {
                        store = staff.Store.StoreName;
                    }
                    StaffModel st = new StaffModel()
                    {
                        StaffID = staff.StaffID,
                        StaffName = staff.StaffName,
                        Gender = staff.Gender,
                        Email = staff.Email,
                        PhoneNumber = staff.PhoneNumber,
                        Address = staff.Address,
                        DateOfBirth = staff.DateOfBirth,
                        GroupName = group,
                        StoreName = store,
                        BaseSalary = staff.BaseSalary
                    };
                    List<Order> ord = staff.Orders.ToList();
                    List<OrderPrice> order = new List<OrderPrice>();
                    foreach (Order o in ord)
                    {
                        OrderPrice odp = new OrderPrice();
                        odp.OrderID = o.OrderID;
                        odp.CustomerName = o.CustomerName;
                        odp.CreatedDate = o.CreatedDate;
                        double? total = 0;
                        foreach (OrderProduct op in o.OrderProducts)
                        {
                            total += op.Quantity * op.Product.UnitPrice;
                        }
                        odp.TotalPrice = total;
                        order.Add(odp);
                    }
                    ViewBag.Order = order;
                    db.Database.Connection.Close();
                    ViewBag.Staff = st;
                    ViewBag.Group = db.UserGroups;
                    ViewBag.Store = db.Stores;
                    ViewBag.Gender = new List<string>() { "Male", "Female" };
                    return View();
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "Something went wrong. Cannot find selected manager. Please try again later";
                    return RedirectToAction("Manager", "Home");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. Cannot find selected manager. Please try again later";
                return RedirectToAction("Manager", "Home");
            }
        }

        [HasCredential(Action = "DELETE_MANAGER")]
        public ActionResult DeleteManager(int id)
        {
            try
            {
                db.Database.Connection.Open();
                Staff delStaff = db.Staffs.SingleOrDefault(s => s.StaffID == id);
                if (delStaff != null)
                {
                    db.Staffs.Remove(delStaff);
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("Manager");
                }
                else
                {
                    TempData["error"] = "The seleted manager is no longer available";
                    db.Database.Connection.Close();
                    return RedirectToAction("Manager");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong, cannot delete selected manager. Please refresh the page or try again later";
                return RedirectToAction("Manager");
            }
        }

        [HasCredential(Action = "VIEW_STAFF")]
        public ActionResult SaleStaff()
        {
            ViewBag.List = GetAllSaleStaff();
            return View();
        }

        public List<StaffModel> GetAllSaleStaff()
        {
            db.Database.Connection.Open();
            List<StaffModel> mng = new List<StaffModel>();
            List<Staff> list = db.Staffs.Where(s => s.GroupID == "STAFF").ToList();
            foreach (Staff m in list)
            {
                string GroupName = null;
                string StoreName = null;
                if (m.GroupID != null)
                {
                    GroupName = db.UserGroups.SingleOrDefault(g => g.GroupID == m.GroupID).GroupName;
                }
                if (m.StoreID != null)
                {
                    StoreName = db.Stores.SingleOrDefault(s => s.StoreID == m.StoreID).StoreName;
                }
                mng.Add(new StaffModel { StaffID = m.StaffID, StaffName = m.StaffName, Gender = m.Gender, Email = m.Email, PhoneNumber = m.PhoneNumber, Address = m.Address, DateOfBirth = m.DateOfBirth, GroupName = GroupName, StoreName = StoreName, BaseSalary = m.BaseSalary });
            }
            db.Database.Connection.Close();
            return mng;
        }

        [HasCredential(Action = "DELETE_STAFF")]
        public ActionResult DeleteStaff(int id)
        {
            try
            {
                db.Database.Connection.Open();
                Staff delStaff = db.Staffs.SingleOrDefault(s => s.StaffID == id);
                if (delStaff != null)
                {
                    db.Staffs.Remove(delStaff);
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("SaleStaff", "Home");
                }
                else
                {
                    TempData["error"] = "The seleted staff is no longer available";
                    db.Database.Connection.Close();
                    return RedirectToAction("SaleStaff", "Home");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong, cannot delete selected staff. Please refresh the page or try again later";
                return RedirectToAction("SaleStaff", "Home");
            }
        }

        [HasCredential(Action = "VIEW_STAFF")]
        public ActionResult StaffDetail(int id)
        {
            try
            {
                db.Database.Connection.Open();
                Staff staff = db.Staffs.SingleOrDefault(p => p.StaffID == id);
                if (staff != null)
                {
                    string group = null;
                    string store = null;
                    if (staff.GroupID != null)
                    {
                        group = staff.UserGroup.GroupName;
                    }
                    if (staff.StoreID != null)
                    {
                        store = staff.Store.StoreName;
                    }
                    StaffModel st = new StaffModel()
                    {
                        StaffID = staff.StaffID,
                        StaffName = staff.StaffName,
                        Gender = staff.Gender,
                        Email = staff.Email,
                        PhoneNumber = staff.PhoneNumber,
                        Address = staff.Address,
                        DateOfBirth = staff.DateOfBirth,
                        GroupName = group,
                        StoreName = store,
                        BaseSalary = staff.BaseSalary
                    };
                    List<Order> ord = staff.Orders.ToList();
                    List<OrderPrice> order = new List<OrderPrice>();
                    foreach (Order o in ord)
                    {
                        OrderPrice odp = new OrderPrice();
                        odp.OrderID = o.OrderID;
                        odp.CustomerName = o.CustomerName;
                        odp.CreatedDate = o.CreatedDate;
                        double? total = 0;
                        foreach (OrderProduct op in o.OrderProducts)
                        {
                            total += op.Quantity * op.Product.UnitPrice;
                        }
                        odp.TotalPrice = total;
                        order.Add(odp);
                    }
                    ViewBag.Order = order;
                    db.Database.Connection.Close();
                    ViewBag.Staff = st;
                    ViewBag.Group = db.UserGroups;
                    ViewBag.Store = db.Stores;
                    ViewBag.Gender = new List<string>() { "Male", "Female" };
                    return View();
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "Something went wrong. Cannot find selected staff. Please try again later";
                    return RedirectToAction("SaleStaff", "Home");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. Cannot find selected staff. Please try again later";
                return RedirectToAction("SaleStaff", "Home");
            }
        }

        [HasCredential(Action = "UPDATE_MANAGER")]
        public ActionResult UpdateManager(StaffModel value)
        {
            try
            {
                db.Database.Connection.Open();
                Staff staff = db.Staffs.SingleOrDefault(s => s.StaffID == value.StaffID);
                if (staff != null)
                {
                    staff.StaffID = value.StaffID;
                    staff.StaffName = value.StaffName;
                    staff.Gender = value.Gender;
                    staff.Email = value.Email;
                    staff.PhoneNumber = value.PhoneNumber;
                    staff.Address = value.Address;
                    staff.DateOfBirth = value.DateOfBirth;
                    staff.GroupID = db.UserGroups.SingleOrDefault(g => g.GroupName == value.GroupName).GroupID;
                    staff.StoreID = db.Stores.SingleOrDefault(s => s.StoreName == value.StoreName).StoreID;
                    staff.BaseSalary = value.BaseSalary;
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("ManagerDetail", "Home", new { id = staff.StaffID });
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "Cannot update the manager. Please try again later or contact your mananger";
                    return RedirectToAction("Manager");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. Contact your manager or try again later";
                return RedirectToAction("Manager");
            }
        }

        [HasCredential(Action = "UPDATE_STAFF")]
        public ActionResult UpdateStaff(StaffModel value)
        {
            try
            {
                db.Database.Connection.Open();
                Staff staff = db.Staffs.SingleOrDefault(s => s.StaffID == value.StaffID);
                if (staff != null)
                {
                    staff.StaffID = value.StaffID;
                    staff.StaffName = value.StaffName;
                    staff.Gender = value.Gender;
                    staff.Email = value.Email;
                    staff.PhoneNumber = value.PhoneNumber;
                    staff.Address = value.Address;
                    staff.DateOfBirth = value.DateOfBirth;
                    staff.GroupID = db.UserGroups.SingleOrDefault(g => g.GroupName == value.GroupName).GroupID;
                    staff.StoreID = db.Stores.SingleOrDefault(s => s.StoreName == value.StoreName).StoreID;
                    staff.BaseSalary = value.BaseSalary;
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("StaffDetail", "Home", new { id = staff.StaffID });
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "Cannot update the manager. Please try again later or contact your mananger";
                    return RedirectToAction("Staff");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. Contact your manager or try again later";
                return RedirectToAction("Staff");
            }
        }

        [HasCredential(Action = "ADD_MANAGER")]
        public ActionResult CreateManager()
        {
            db.Database.Connection.Open();
            ViewBag.Group = db.UserGroups;
            ViewBag.Store = db.Stores;
            db.Database.Connection.Close();
            ViewBag.Gender = new List<string>() { "Male", "Female" };
            return View();
        }

        [HasCredential(Action = "ADD_MANAGER")]
        [HttpPost]
        public ActionResult CreateManager(Staff value)
        {
            Staff st = new Staff();
            st.StaffName = value.StaffName;
            st.Password = MD5Encryptor.MD5Hash(value.Password);
            st.Gender = value.Gender;
            st.Address = value.Address;
            st.DateOfBirth = value.DateOfBirth;
            st.Email = value.Email;
            st.GroupID = value.GroupID;
            st.PhoneNumber = value.PhoneNumber;
            st.StoreID = value.StoreID;
            st.BaseSalary = value.BaseSalary;
            try
            {
                db.Database.Connection.Open();
                db.Staffs.Add(st);
                db.SaveChanges();
                db.Database.Connection.Close();
                return RedirectToAction("ManagerDetail", new { id = st.StaffID });
            }
            catch
            {
                TempData["error"] = "Something went wrong. Please contact your manager or try again later";
            }
            return RedirectToAction("Manager");
        }

        [HasCredential(Action = "ADD_STAFF")]
        public ActionResult CreateStaff()
        {
            db.Database.Connection.Open();
            ViewBag.Group = db.UserGroups;
            ViewBag.Store = db.Stores;
            db.Database.Connection.Close();
            ViewBag.Gender = new List<string>() { "Male", "Female" };
            return View();
        }

        [HasCredential(Action = "ADD_STAFF")]
        [HttpPost]
        public ActionResult CreateStaff(Staff value)
        {
            Staff st = new Staff();
            st.StaffName = value.StaffName;
            st.Password = MD5Encryptor.MD5Hash(value.Password);
            st.Gender = value.Gender;
            st.Address = value.Address;
            st.DateOfBirth = value.DateOfBirth;
            st.Email = value.Email;
            st.GroupID = value.GroupID;
            st.PhoneNumber = value.PhoneNumber;
            st.StoreID = value.StoreID;
            st.BaseSalary = value.BaseSalary;
            try
            {
                db.Database.Connection.Open();
                db.Staffs.Add(st);
                db.SaveChanges();
                db.Database.Connection.Close();
                return RedirectToAction("StaffDetail", new { id = st.StaffID });
            }
            catch
            {
                TempData["error"] = "Something went wrong. Please contact your manager or try again later";
            }
            return RedirectToAction("SaleStaff");
        }

        [HasCredential(Action = "VIEW_STORE")]
        public ActionResult Stores()
        {
            db.Database.Connection.Open();
            ViewBag.List = db.Stores.ToList();
            db.Database.Connection.Close();
            return View();
        }

        [HasCredential(Action = "UPDATE_STORE")]
        public ActionResult EditStore(int id)
        {
            db.Database.Connection.Open();
            ViewBag.Store = db.Stores.SingleOrDefault(s => s.StoreID == id);
            db.Database.Connection.Close();
            return View();
        }

        [HasCredential(Action = "UPDATE_STORE")]
        [HttpPost]
        public ActionResult UpdateStore(Store value)
        {
            try
            {
                db.Database.Connection.Open();
                Store st = db.Stores.SingleOrDefault(s => s.StoreID == value.StoreID);
                if (st != null)
                {
                    st.StoreName = value.StoreName;
                    st.StoreEmail = value.StoreEmail;
                    st.StoreAddress = value.StoreAddress;
                    st.StorePhone = value.StorePhone;
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("Stores");
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "Something went wrong. Cannot find the selected store. Please try again later or contact your manager";
                    return RedirectToAction("Stores");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. Please try again later or contact your manager";
                return RedirectToAction("Stores");
            }
        }

        [HasCredential(Action = "ADD_STORE")]
        public ActionResult CreateStore(Store value)
        {
            try
            {
                db.Database.Connection.Open();
                db.Stores.Add(value);
                db.SaveChanges();
                db.Database.Connection.Close();
                return RedirectToAction("Stores");
            }
            catch
            {
                TempData["error"] = "Something went wrong. Please contact your manager or try again later";
            }
            return RedirectToAction("Stores");
        }

        [HasCredential(Action = "DELETE_STORE")]
        public ActionResult DeleteStore(int id)
        {
            try
            {
                db.Database.Connection.Open();
                Store st = db.Stores.SingleOrDefault(s => s.StoreID == id);
                if (st != null)
                {
                    db.Stores.Remove(st);
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("Stores");
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "Cannot delete the store. Please try again later or contact your mananger";
                    return RedirectToAction("Stores");
                }
            }
            catch
            {
                TempData["error"] = "Cannot delete the store. Please try again later or contact your mananger";
                return RedirectToAction("Stores");
            }
        }

        [HasCredential(Action = "VIEW_ORDER")]
        public ActionResult Orders()
        {
            ViewBag.List = this.GetAllOrder();
            return View();
        }

        private List<OrderModel> GetAllOrder()
        {
            List<OrderModel> list = new List<OrderModel>();
            db.Database.Connection.Open();
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
            db.Database.Connection.Close();
            return list;
        }

        [HasCredential(Action = "VIEW_ORDER")]
        public ActionResult OrderDetail(int id)
        {
            TempData["id"] = id;
            return View();
        }

        private OrderModel GetOrderById(int id)
        {
            OrderModel order = new OrderModel();
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
            order.Status = od.OrderStatu.StatusName;
            order.PaymentMethodID = od.PaymentMethodID;
            db.Database.Connection.Close();
            return order;
        }

        private List<CartModel> GetOrderProductByOrderId(int id)
        {
            List<CartModel> list = new List<CartModel>();
            db.Database.Connection.Open();
            List<OrderProduct> product = db.OrderProducts.Where(o => o.OrderID == id).ToList();
            foreach (OrderProduct p in product)
            {
                list.Add(new CartModel { ProductID = p.ProductID, ProductName = p.Product.ProductName, Quantity = p.Quantity, UnitPrice = p.Product.UnitPrice });
            }
            db.Database.Connection.Close();
            return list;
        }

        [HasCredential(Action = "ADD_ORDER")]
        public ActionResult AddOrder()
        {
            ViewBag.Payment = db.PaymentMethods;
            return View();
        }

        private Product GetProductById(int id)
        {
            Product pro = db.Products.SingleOrDefault(p => p.ProductID == id);
            return pro;
        }

        public ActionResult AddItem(int productID)
        {
            db.Database.Connection.Open();
            Product product = GetProductById(productID);
            if (product != null)
            {
                var cart = Session[CommonConstants.CART_SESSION];
                if (cart != null)
                {
                    List<CartModel> list = (List<CartModel>)cart;
                    if (list.Exists(m => m.ProductID == productID))
                    {
                        CartModel item = list.SingleOrDefault(m => m.ProductID == productID);
                        if (item.Quantity == product.UnitInStock)
                        {
                            TempData["error"] = "The input quantity is greater than in-stock products";
                        }
                        else
                        {
                            item.Quantity += 1;
                        }
                        db.Database.Connection.Close();
                    }
                    else
                    {
                        CartModel item = new CartModel();
                        item.ProductID = product.ProductID;
                        item.ProductName = product.ProductName;
                        item.UnitPrice = product.UnitPrice;
                        item.Quantity = 1;

                        //add item to list
                        list.Add(item);
                        db.Database.Connection.Close();
                    }
                }
                else
                {
                    if (product.UnitInStock > 0)
                    {
                        List<CartModel> list = new List<CartModel>();

                        //create new item
                        CartModel item = new CartModel();
                        item.ProductID = product.ProductID;
                        item.ProductName = product.ProductName;
                        item.UnitPrice = product.UnitPrice;
                        item.Quantity = 1;

                        //add item to list
                        list.Add(item);

                        //save list to session
                        Session.Add(CommonConstants.CART_SESSION, list);
                    }
                    else
                    {
                        TempData["error"] = "The product is out of stock";
                    }
                    db.Database.Connection.Close();
                }
            }
            else
            {
                TempData["error"] = "The product is not existing";
            }
            return RedirectToAction("AddOrder", "Home");
        }

        public ActionResult EditQuant(int id, int quantity)
        {
            List<CartModel> list = (List<CartModel>)Session[CommonConstants.CART_SESSION];
            Product product = db.Products.SingleOrDefault(p => p.ProductID == id);
            if (list.Exists(m => m.ProductID == id))
            {
                CartModel pro = list.SingleOrDefault(m => m.ProductID == id);
                if (quantity > product.UnitInStock)
                {
                    TempData["error"] = "The input quantity is greater than in-stock products";
                }
                else
                {
                    pro.Quantity = quantity;
                }
                return RedirectToAction("AddOrder", "Home");
            }
            else
            {
                TempData["error"] = "Cannot edit quantity";
                return RedirectToAction("AddOrder", "Home");
            }
        }

        public ActionResult DeleteItem(int id)
        {
            List<CartModel> cart = (List<CartModel>)Session[CommonConstants.CART_SESSION];
            if (cart.Exists(p => p.ProductID == id))
            {
                cart.Remove(cart.SingleOrDefault(p => p.ProductID == id));
                return RedirectToAction("AddOrder", "Home");
            }
            else
            {
                return RedirectToAction("AddOrder", "Home");
            }
        }

        [HasCredential(Action = "ADD_ORDER")]
        [HttpPost]
        public ActionResult CreateOrder(Order order)
        {
            db.Database.Connection.Open();
            order.PaymentMethodID = order.PaymentMethodID;
            order.StatusID = 5;
            order.StaffID = ((Staff)Session[CommonConstants.USER_SESSION]).StaffID;
            order.CreatedDate = DateTime.Now;
            db.Orders.Add(order);
            List<CartModel> list = (List<CartModel>)Session[CommonConstants.CART_SESSION];
            if (list != null)
            {
                foreach (CartModel cart in list)
                {
                    OrderProduct op = new OrderProduct();
                    op.ProductID = cart.ProductID;
                    op.OrderID = order.OrderID;
                    op.Quantity = cart.Quantity;
                    db.OrderProducts.Add(op);
                    Product product = db.Products.SingleOrDefault(p => p.ProductID == cart.ProductID);
                    product.UnitInStock = product.UnitInStock - cart.Quantity;
                }
                db.SaveChanges();
                Session.Remove(CommonConstants.CART_SESSION);
                db.Database.Connection.Close();
            }
            else
            {
                TempData["error"] = "No product is added";
            }
            return RedirectToAction("AddOrder", "Home");
        }

        [HasCredential(Action = "UPDATE_ORDER")]
        [HttpPost]
        public ActionResult EditOrder(OrderModel order)
        {
            try
            {
                db.Database.Connection.Open();
                Order ord = db.Orders.SingleOrDefault(o => o.OrderID == order.OrderID);
                if (ord != null)
                {
                    ord.CustomerName = order.CustomerName;
                    ord.ContactPhone = order.ContactPhone;
                    ord.Note = order.Note;
                    ord.ShippingAddress = order.ShippingAddress;
                    ord.StatusID = db.OrderStatus.Single(stt => stt.StatusName == order.Status).StatusID;
                    ord.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return RedirectToAction("OrderDetail", "Home", routeValues: new { id = ord.OrderID });
                }
                else
                {
                    db.Database.Connection.Close();
                    TempData["error"] = "The order is no longer available";
                    return RedirectToAction("Orders", "Home");
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. Try again later";
                return RedirectToAction("OrderDetail", "Home", routeValues: new { id = order.OrderID });
            }
        }

        [HasCredential(Action = "VIEW_REPORT")]
        public ActionResult ReportSalary()
        {
            db.Database.Connection.Open();
            List<Salary> list = new List<Salary>();
            List<Staff> listStaff = db.Staffs.Where(s => s.GroupID != "ADMIN").ToList();
            foreach (Staff staff in listStaff)
            {
                Salary salary = new Salary();
                salary.StaffID = staff.StaffID;
                salary.StaffName = staff.StaffName;
                salary.BaseSalary = staff.BaseSalary;
                List<Order> listOrder = db.Orders.Where(o => o.StaffID == staff.StaffID).ToList();
                foreach (Order ord in listOrder)
                {
                    List<OrderProduct> productList = ord.OrderProducts.ToList();
                    foreach (OrderProduct op in productList)
                    {
                        int? quant = op.Quantity;
                        double? price = op.Product.UnitPrice;
                        double? bonus = (quant * price) * 0.02;
                        salary.Bonus = bonus;
                    }
                }
                list.Add(salary);
            }
            ViewBag.List = list;
            db.Database.Connection.Close();
            return View();
        }

        [HasCredential(Action = "VIEW_REPORT")]
        public ActionResult ViewReportSalaryByDate(DateTime from, DateTime to)
        {
            db.Database.Connection.Open();
            List<Salary> list = new List<Salary>();
            List<Staff> listStaff = db.Staffs.ToList();
            foreach (Staff staff in listStaff)
            {
                Salary salary = new Salary();
                salary.StaffID = staff.StaffID;
                salary.StaffName = staff.StaffName;
                salary.BaseSalary = staff.BaseSalary;
                List<Order> listOrder = db.Orders.Where(o => (o.StaffID == staff.StaffID) && (from <= o.CreatedDate) && (o.CreatedDate <= to)).ToList();
                foreach (Order ord in listOrder)
                {
                    List<OrderProduct> productList = ord.OrderProducts.ToList();
                    foreach (OrderProduct op in productList)
                    {
                        int? quant = op.Quantity;
                        double? price = op.Product.UnitPrice;
                        double? bonus = (quant * price) * 0.02;
                        salary.Bonus = bonus;
                    }
                }
                list.Add(salary);
            }
            ViewBag.List = list;
            db.Database.Connection.Close();
            return View("ReportSalary");
        }

        [HasCredential(Action = "VIEW_REPORT")]
        public ActionResult ViewReportSalaryByMonth(DateTime month)
        {
            int date;
            DateTime to;
            DateTime from = new DateTime(month.Year, month.Month, 01);
            if ((month.Month == 2) && (month.Year % 4 == 0))
            {
                date = 29;
                to = new DateTime(month.Year, month.Month, date);
            }
            else if ((month.Month == 2) && (month.Year % 4 != 0))
            {
                date = 28;
                to = new DateTime(month.Year, month.Month, date);
            }
            else if (month.Month == 1 || month.Month == 3 || month.Month == 5 || month.Month == 7 || month.Month == 8 || month.Month == 10 || month.Month == 12)
            {
                date = 31;
                to = new DateTime(month.Year, month.Month, date);
            }
            else
            {
                date = 30;
                to = new DateTime(month.Year, month.Month, date);
            }
            db.Database.Connection.Open();
            List<Salary> list = new List<Salary>();
            List<Staff> listStaff = db.Staffs.ToList();
            foreach (Staff staff in listStaff)
            {
                Salary salary = new Salary();
                salary.StaffID = staff.StaffID;
                salary.StaffName = staff.StaffName;
                salary.BaseSalary = staff.BaseSalary;
                List<Order> listOrder = db.Orders.Where(o => (o.StaffID == staff.StaffID) && (from <= o.CreatedDate) && (o.CreatedDate <= to)).ToList();
                foreach (Order ord in listOrder)
                {
                    List<OrderProduct> productList = ord.OrderProducts.ToList();
                    foreach (OrderProduct op in productList)
                    {
                        int? quant = op.Quantity;
                        double? price = op.Product.UnitPrice;
                        double? bonus = (quant * price) * 0.02;
                        salary.Bonus = bonus;
                    }
                }
                list.Add(salary);
            }
            ViewBag.List = list;
            db.Database.Connection.Close();
            return View("ReportSalary");
        }

        [HasCredential(Action = "VIEW_REPORT")]
        public ActionResult ViewReportRevenue()
        {
            db.Database.Connection.Open();
            List<StoreRevenue> list = new List<StoreRevenue>();
            foreach (Store st in db.Stores)
            {
                StoreRevenue s = new StoreRevenue();
                s.StoreID = st.StoreID;
                s.StoreName = st.StoreName;
                List<OrderPrice> listOdp = new List<OrderPrice>();
                foreach (Order ord in db.Orders.Where(o => o.Staff.StoreID == st.StoreID && o.StatusID == 5).ToList())
                {
                    OrderPrice op = new OrderPrice();
                    op.OrderID = ord.OrderID;
                    op.CustomerName = ord.CustomerName;
                    op.CreatedDate = ord.CreatedDate;
                    List<OrderProduct> products = ord.OrderProducts.ToList();
                    double? total = 0;
                    foreach (OrderProduct odp in products)
                    {
                        total += odp.Product.UnitPrice * odp.Quantity;
                    }
                    op.TotalPrice = total;
                    listOdp.Add(op);
                }
                s.Order = listOdp;
                list.Add(s);
            }
            ViewBag.Store = list;
            db.Database.Connection.Close();
            return View();
        }

        [HasCredential(Action = "VIEW_REPORT")]
        public ActionResult ViewReportRevenueByDate(DateTime from, DateTime to)
        {
            db.Database.Connection.Open();
            List<StoreRevenue> list = new List<StoreRevenue>();
            foreach (Store st in db.Stores)
            {
                StoreRevenue s = new StoreRevenue();
                s.StoreID = st.StoreID;
                s.StoreName = st.StoreName;
                List<OrderPrice> listOdp = new List<OrderPrice>();
                foreach (Order ord in db.Orders.Where(o => o.Staff.StoreID == st.StoreID && (o.CreatedDate >= from) && (o.CreatedDate <= to) && o.StatusID == 5).ToList())
                {
                    OrderPrice op = new OrderPrice();
                    op.OrderID = ord.OrderID;
                    op.CustomerName = ord.CustomerName;
                    op.CreatedDate = ord.CreatedDate;
                    List<OrderProduct> products = ord.OrderProducts.ToList();
                    double? total = 0;
                    foreach (OrderProduct odp in products)
                    {
                        total += odp.Product.UnitPrice * odp.Quantity;
                    }
                    op.TotalPrice = total;
                    listOdp.Add(op);
                }
                s.Order = listOdp;
                list.Add(s);
            }
            ViewBag.Store = list;
            db.Database.Connection.Close();
            return View("ViewReportRevenue");
        }

        [HasCredential(Action = "VIEW_REPORT")]
        public ActionResult ViewReportRevenueByMonth(DateTime month)
        {
            int date;
            DateTime to;
            DateTime from = new DateTime(month.Year, month.Month, 01);
            if ((month.Month == 2) && (month.Year % 4 == 0))
            {
                date = 29;
                to = new DateTime(month.Year, month.Month, date);
            }
            else if ((month.Month == 2) && (month.Year % 4 != 0))
            {
                date = 28;
                to = new DateTime(month.Year, month.Month, date);
            }
            else if (month.Month == 1 || month.Month == 3 || month.Month == 5 || month.Month == 7 || month.Month == 8 || month.Month == 10 || month.Month == 12)
            {
                date = 31;
                to = new DateTime(month.Year, month.Month, date);
            }
            else
            {
                date = 30;
                to = new DateTime(month.Year, month.Month, date);
            }
            db.Database.Connection.Open();
            List<StoreRevenue> list = new List<StoreRevenue>();
            foreach (Store st in db.Stores)
            {
                StoreRevenue s = new StoreRevenue();
                s.StoreID = st.StoreID;
                s.StoreName = st.StoreName;
                List<OrderPrice> listOdp = new List<OrderPrice>();
                foreach (Order ord in db.Orders.Where(o => o.Staff.StoreID == st.StoreID && (o.CreatedDate >= from) && (o.CreatedDate <= to) && o.StatusID == 5).ToList())
                {
                    OrderPrice op = new OrderPrice();
                    op.OrderID = ord.OrderID;
                    op.CustomerName = ord.CustomerName;
                    op.CreatedDate = ord.CreatedDate;
                    List<OrderProduct> products = ord.OrderProducts.ToList();
                    double? total = 0;
                    foreach (OrderProduct odp in products)
                    {
                        total += odp.Product.UnitPrice * odp.Quantity;
                    }
                    op.TotalPrice = total;
                    listOdp.Add(op);
                }
                s.Order = listOdp;
                list.Add(s);
            }
            ViewBag.Store = list;
            db.Database.Connection.Close();
            return View("ViewReportRevenue");
        }

        public ActionResult ViewProfile(int id)
        {
            db.Database.Connection.Open();
            Staff staff = db.Staffs.SingleOrDefault(p => p.StaffID == id);
            if (staff != null)
            {
                string group = null;
                string store = null;
                if (staff.GroupID != null)
                {
                    group = staff.UserGroup.GroupName;
                }
                if (staff.StoreID != null)
                {
                    store = staff.Store.StoreName;
                }
                StaffModel st = new StaffModel()
                {
                    StaffID = staff.StaffID,
                    StaffName = staff.StaffName,
                    Gender = staff.Gender,
                    Email = staff.Email,
                    PhoneNumber = staff.PhoneNumber,
                    Address = staff.Address,
                    DateOfBirth = staff.DateOfBirth,
                    GroupName = group,
                    StoreName = store
                };
                List<Order> ord = staff.Orders.ToList();
                List<OrderPrice> order = new List<OrderPrice>();
                foreach (Order o in ord)
                {
                    OrderPrice odp = new OrderPrice();
                    odp.OrderID = o.OrderID;
                    odp.CustomerName = o.CustomerName;
                    odp.CreatedDate = o.CreatedDate;
                    double? total = 0;
                    foreach (OrderProduct op in o.OrderProducts)
                    {
                        total += op.Quantity * op.Product.UnitPrice;
                    }
                    odp.TotalPrice = total;
                    order.Add(odp);
                }
                ViewBag.Order = order;
                db.Database.Connection.Close();
                ViewBag.Staff = st;
                ViewBag.Group = db.UserGroups;
                ViewBag.Store = db.Stores;
                ViewBag.Gender = new List<string>() { "Male", "Female" };
            }
            return View();
        }

        public ActionResult CancelCart()
        {
            Session.Remove(CommonConstants.CART_SESSION);
            return RedirectToAction("AddOrder", "Home");
        }

        public ActionResult PrintSalaryReport(string time, string salary)
        {
            List<Salary> list = JsonConvert.DeserializeObject<List<Salary>>(salary);
            ViewBag.Salary = list;
            ViewBag.Time = time;
            return View();
        }

        public ActionResult PrintOrderDetail(int id)
        {
            TempData["id"] = id;
            return View();
        }
    }
}