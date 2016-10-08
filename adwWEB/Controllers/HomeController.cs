using adwWEB.Common;
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
        public ActionResult MyChart(string nazwa, string wartosc)
        {
            List<int> zbior = new List<int>();
            zbior.Add(1);
            zbior.Add(3);
            zbior.Add(7);
            zbior.Add(9);
            zbior.Add(11);
            zbior.Add(13);
            zbior.Add(17);
            zbior.Add(19);

            zbior.Add(21);
            zbior.Add(23);
            zbior.Add(27);
            zbior.Add(29);
            List<int> wynik = classFun.Losuj(zbior, 4);
            string[] toktnazwa = nazwa.Split(',');
            string[] toktwartosc = wartosc.Split(',');

            var testChart = new Chart(width: 600, height: 400)
                .AddTitle("Price history")
                .AddSeries(
                    name: "Price",
         legend: "Price",
                    axisLabel: "#VALY",
                    xValue: toktnazwa,
                    yValues: toktwartosc)
                .GetBytes("png");

            //chart.Series[seriesName].IsValueShownAsLabel = true;
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
            if (product.ProductProductPhoto.Count == 1)
            {
                if (product.ProductProductPhoto.ToList()[0].ProductPhotoID != 1)
                    return true;
            }
            return false;
        }

  
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);

            var resultProduct = (from Product in db.Product.Where(k => k.ProductID == id)
                           join ProductSubcategory in db.ProductSubcategory on new { ProductSubcategoryID = (int)Product.ProductSubcategoryID } equals new { ProductSubcategoryID = ProductSubcategory.ProductSubcategoryID } into ProductSubcategory_join
                           from ProductSubcategory in ProductSubcategory_join.DefaultIfEmpty()
                           join ProductCategory in db.ProductCategory on ProductSubcategory.ProductCategoryID equals ProductCategory.ProductCategoryID into ProductCategory_join
                           from ProductCategory in ProductCategory_join.DefaultIfEmpty()
                           select new ProductViewList
                           {
                               Id = Product.ProductID,
                               Product = Product,
                          
                               ProductSubcategory=ProductSubcategory.ProductSubcategoryID,
                               ProductSubcategoryName = ProductSubcategory != null ? ProductSubcategory.Name : null,
                               ProductCategoryName = ProductCategory != null ? ProductCategory.Name : null,
                               WithPhoto = Product.ProductProductPhoto.Count != 1 ? false : Product.ProductProductPhoto.FirstOrDefault().ProductPhotoID != 1 ? true : false


                           });

            ProductViewList result = resultProduct.FirstOrDefault();

            var randomProduct = from p in db.Product
                           join ProductSubcategory in db.ProductSubcategory.Where(k => k.ProductSubcategoryID == result.ProductSubcategory)
                            on new { ProductSubcategoryID = (int)p.ProductSubcategoryID } equals new { ProductSubcategoryID = ProductSubcategory.ProductSubcategoryID }
                           select p.ProductID;
            List<int> wynik = classFun.Losuj(randomProduct.ToList(), 5);

            result.ProductListPriceHistory = db.ProductListPriceHistory.Where(k => k.ProductID == id).ToList();
            result.WithPhoto = product.ProductProductPhoto.Count != 1 ? false : product.ProductProductPhoto.FirstOrDefault().ProductPhotoID != 1 ? true : false;
           
            result.QuanityAll = db.ProductInventory.Where(k => k.ProductID == id).ToList().Sum(k => k.Quantity);
            result.ProductRandom = db.Product.Where(k => wynik.Contains(k.ProductID)).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
    }
}