using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Datos.Datos;

namespace Principal.Controllers
{
    public class ProductController : Controller
    {
        private NorthWindW3DbContext db = new NorthWindW3DbContext();

        // GET: Producto
        public ActionResult Index(int? id, bool? categoria)
        {
            var productos = db.Products.Include(p => p.Categories).Include(p => p.suppliers);
          
            if (id != null && id > 0)
            {
                if (categoria != null)
                {
                    if (categoria == true)
                    {
                        productos = productos.Where(x => x.CategoryID == id);
                        if (productos != null && productos.Count() > 0)
                        {
                            ViewBag.Message = "Nombre de la categoria " + productos.FirstOrDefault().Categories.CategoryName + " Descripcion de la categoria " + productos.FirstOrDefault().Categories.Description;
                        }
                    }
                    else
                    {
                        productos = productos.Where(x => x.supplierID == id);
                        if (productos != null && productos.Count() > 0)
                        {
                            ViewBag.Message = "Nombre del proveedor " + productos.FirstOrDefault().suppliers.supplierName+ " Nombre de contacto " + productos.FirstOrDefault().suppliers.ContactName;
                        }
                    }
                }
            }
            return View(productos.ToList());
        }


        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.supplierID = new SelectList(db.suppliers, "supplierID", "supplierName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,supplierID,CategoryID,unit,Price")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.supplierID = new SelectList(db.suppliers, "supplierID", "supplierName", products.supplierID);
            return View(products);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.supplierID = new SelectList(db.suppliers, "supplierID", "supplierName", products.supplierID);
            return View(products);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,supplierID,CategoryID,unit,Price")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.supplierID = new SelectList(db.suppliers, "supplierID", "supplierName", products.supplierID);
            return View(products);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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
