using adwWEB.Models;
using adwWEB.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace adwWEB.Controllers
{
    public class HomeController : Controller
    {
        private adwentureEntities db = new adwentureEntities();
        public ActionResult MyChart(string nazwa,string wartosc)
        {
            string[] toktnazwa = nazwa.Split(',');
            string[] toktwartosc = wartosc.Split(',');
            var testChart = new Chart(width: 600, height: 400)
                .AddTitle("Test")
                .AddSeries(
                    name: "Employee",
                    xValue: toktnazwa,
                    yValues: toktwartosc)
                .GetBytes("png");
            return File(testChart, "image/png");
        }
        public ActionResult Index(int? page, string sub, string cat)
        {
            /*
                  <text>Subcategory</text>@Html.DropDownList("sub", new SelectList(ViewBag.sub))
        <text>Category</text>@Html.DropDownList("cat", new SelectList(ViewBag.cat))
        <input type="submit" value="search" />
             */
            ViewBag.cat1 = cat;
            ViewBag.sub1 = sub;
            ViewBag.cat = db.ProductCategory.Select(k => k.Name);
            ViewBag.sub = db.ProductSubcategory.Select(k => k.Name);
            var result1 = (from Product in db.Product
                           join ProductSubcategory in db.ProductSubcategory on new { ProductSubcategoryID = (int)Product.ProductSubcategoryID } equals new { ProductSubcategoryID = ProductSubcategory.ProductSubcategoryID } into ProductSubcategory_join
                           from ProductSubcategory in ProductSubcategory_join.DefaultIfEmpty()
                           join ProductCategory in db.ProductCategory on ProductSubcategory.ProductCategoryID equals ProductCategory.ProductCategoryID into ProductCategory_join
                           from ProductCategory in ProductCategory_join.DefaultIfEmpty()

                           select new ProductViewList
                           {
                               Id = Product.ProductID,
                               Product = Product,

                               ProductSubcategoryName = ProductSubcategory != null ? ProductSubcategory.Name : null,
                               ProductCategoryName = ProductCategory != null ? ProductCategory.Name : null,
                               WithPhoto = Product.ProductProductPhoto.Count != 1 ? false : Product.ProductProductPhoto.FirstOrDefault().ProductPhotoID != 1 ? true : false
                               

                           }).ToList();
            if (cat != null)
            {
                if (cat != "")
                    result1 = result1.Where(k => k.ProductCategoryName == cat).ToList();
            }
            if (sub != null)
            {
                if (sub != "")
                    result1 = result1.Where(k => k.ProductSubcategoryName == sub).ToList();
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(result1.ToPagedList(pageNumber, pageSize));
        }

        private bool HavePhoto(Product product)
        {
            if(product.ProductProductPhoto.Count==1)
            {
                if (product.ProductProductPhoto.ToList()[0].ProductPhotoID != 1)
                    return true;
            }
            return false;
        }

        //wywalismy start
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            ProductViewList result = new ProductViewList();
            result.Product = product;
            result.ProductCostHistory=db.ProductCostHistory.Where(k => k.ProductID == id).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
    }
}