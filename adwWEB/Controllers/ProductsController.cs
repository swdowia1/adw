using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using adwWEB.Models;
using adwWEB.ViewModel;
using PagedList;

namespace adwWEB.Controllers
{
    public class ProductsController : Controller
    {
        private adwentureEntities db = new adwentureEntities();

        // GET: Products
        public ActionResult Index(int? page,string sub,string cat)
        {
            /*
                  <text>Subcategory</text>@Html.DropDownList("sub", new SelectList(ViewBag.sub))
        <text>Category</text>@Html.DropDownList("cat", new SelectList(ViewBag.cat))
        <input type="submit" value="search" />
             */
            ViewBag.cat = db.ProductCategory.Select(k=>k.Name);
            ViewBag.sub = db.ProductSubcategory.Select(k => k.Name);
            var result1 = (from Product in db.Product
                          join ProductSubcategory in db.ProductSubcategory on new { ProductSubcategoryID = (int)Product.ProductSubcategoryID } equals new { ProductSubcategoryID = ProductSubcategory.ProductSubcategoryID } into ProductSubcategory_join
                          from ProductSubcategory in ProductSubcategory_join.DefaultIfEmpty()
                          join ProductCategory in db.ProductCategory on ProductSubcategory.ProductCategoryID equals ProductCategory.ProductCategoryID into ProductCategory_join
                          from ProductCategory in ProductCategory_join.DefaultIfEmpty()
                          select new ProductViewList
                          {
                              Id = Product.ProductID,
                              NameProduct = Product.Name,

                              ProductSubcategoryName = ProductSubcategory != null ? ProductSubcategory.Name : null,
                              ProductCategoryName = ProductCategory != null ? ProductCategory.Name : null

                          }).ToList();
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(result1.ToPagedList(pageNumber, pageSize));
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
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,ProductNumber,MakeFlag,FinishedGoodsFlag,Color,SafetyStockLevel,ReorderPoint,StandardCost,ListPrice,Size,SizeUnitMeasureCode,WeightUnitMeasureCode,Weight,DaysToManufacture,ProductLine,Class,Style,ProductSubcategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,ProductNumber,MakeFlag,FinishedGoodsFlag,Color,SafetyStockLevel,ReorderPoint,StandardCost,ListPrice,Size,SizeUnitMeasureCode,WeightUnitMeasureCode,Weight,DaysToManufacture,ProductLine,Class,Style,ProductSubcategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
