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
    public class CustomBuiltController : Controller
    {
        private DreamTourContext db = new DreamTourContext();


        // GET: Custom_Built
        public ActionResult Index()
        {
            return View(db.Custom_Builts.ToList());
        }

        // GET: Custom_Built/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custom_Built custom_Built = db.Custom_Builts.Find(id);
            if (custom_Built == null)
            {
                return HttpNotFound();
            }
            return View(custom_Built);
        }

        // GET: Custom_Built/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Custom_Built/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,CustomizablePackages,PackageID,PackageType,Status,TourFare")] Custom_Built custom_Built)
        {
            if (ModelState.IsValid)
            {
                db.Custom_Builts.Add(custom_Built);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(custom_Built);
        }

        // GET: Custom_Built/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custom_Built custom_Built = db.Custom_Builts.Find(id);
            if (custom_Built == null)
            {
                return HttpNotFound();
            }
            return View(custom_Built);
        }

        // POST: Custom_Built/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,CustomizablePackages,PackageID,PackageType,Status,TourFare")] Custom_Built custom_Built)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custom_Built).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(custom_Built);
        }

        // GET: Custom_Built/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custom_Built custom_Built = db.Custom_Builts.Find(id);
            if (custom_Built == null)
            {
                return HttpNotFound();
            }
            return View(custom_Built);
        }

        // POST: Custom_Built/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Custom_Built custom_Built = db.Custom_Builts.Find(id);
            db.Custom_Builts.Remove(custom_Built);
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
