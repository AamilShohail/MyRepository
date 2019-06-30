using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourOperator.Models;

namespace TourOperator.Controllers
{
    public class DefaultController : Controller
    {
        private DreamTourContext db = new DreamTourContext();


        public ActionResult DefaultPackage(string dpackage)
        {
            var package = db.Defaults.Where(model => model.Status == dpackage).Distinct().ToList();
            return View(package);
        }


        // GET: Default
        public ActionResult Index()
        {
            return View(db.Defaults.ToList());
        }

        // GET: Default/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Default @default = db.Defaults.Find(id);
            if (@default == null)
            {
                return HttpNotFound();
            }
            return View(@default);
        }

        // GET: Default/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Default/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,DefaultPackages,PackageID,PackageType,Status,TourFare")] Default @default)
        {
            if (ModelState.IsValid)
            {
                db.Defaults.Add(@default);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@default);
        }

        // GET: Default/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Default @default = db.Defaults.Find(id);
            if (@default == null)
            {
                return HttpNotFound();
            }
            return View(@default);
        }

        // POST: Default/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,DefaultPackages,PackageID,PackageType,Status,TourFare")] Default @default)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@default).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@default);
        }

        // GET: Default/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Default @default = db.Defaults.Find(id);
            if (@default == null)
            {
                return HttpNotFound();
            }
            return View(@default);
        }

        // POST: Default/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Default @default = db.Defaults.Find(id);
            db.Defaults.Remove(@default);
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
