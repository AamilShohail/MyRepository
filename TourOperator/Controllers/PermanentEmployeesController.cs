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
    public class PermanentEmployeesController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();

        // GET: PermanentEmployees
        public ActionResult Index()
        {
            return View(db.PermanentEmployees.ToList());
        }

        // GET: PermanentEmployees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermanentEmployee permanentEmployee = db.PermanentEmployees.Find(id);
            if (permanentEmployee == null)
            {
                return HttpNotFound();
            }
            return View(permanentEmployee);
        }

        // GET: PermanentEmployees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PermanentEmployees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AnuualSalary,FirstName,LastName,Gender")] PermanentEmployee permanentEmployee)
        {
            if (ModelState.IsValid)
            {
                db.PermanentEmployees.Add(permanentEmployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(permanentEmployee);
        }

        // GET: PermanentEmployees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermanentEmployee permanentEmployee = db.PermanentEmployees.Find(id);
            if (permanentEmployee == null)
            {
                return HttpNotFound();
            }
            return View(permanentEmployee);
        }

        // POST: PermanentEmployees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AnuualSalary,FirstName,LastName,Gender")] PermanentEmployee permanentEmployee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permanentEmployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permanentEmployee);
        }

        // GET: PermanentEmployees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermanentEmployee permanentEmployee = db.PermanentEmployees.Find(id);
            if (permanentEmployee == null)
            {
                return HttpNotFound();
            }
            return View(permanentEmployee);
        }

        // POST: PermanentEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PermanentEmployee permanentEmployee = db.PermanentEmployees.Find(id);
            db.PermanentEmployees.Remove(permanentEmployee);
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
