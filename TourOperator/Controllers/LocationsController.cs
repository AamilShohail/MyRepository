using PagedList;
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
    public class LocationsController : Controller
    {
        private DreamTourContext db = new DreamTourContext();



        public ActionResult PlanTrip()
        {
            return View();
        }
        public ActionResult InfamousLocation()
        {
            return View();
        }

        public void GoogleMap(string SearchDistrict, string SearchProvince)
        {
            List<Location> places = db.Locations.Where(model => model.Province == SearchProvince && model.District == SearchDistrict).ToList();
            ViewBag.Places = places;
        }

        public ActionResult Gmap(string SearchDistrict, string SearchProvince)
        {
           
            List<Location> places = db.Locations.Where(model => model.Province == SearchProvince && model.District == SearchDistrict).ToList();
            if (SearchProvince == null || SearchProvince == "")
            {
                string error = "No such Search Criteria or unmatching province and district";
                ViewBag.Message = error;
            }
            else
            {
                ViewBag.Places = places;
            }
            List<Location> provinces = db.Locations.GroupBy(model => model.Province).Select(model => model.FirstOrDefault()).ToList();
            ViewBag.ListofDropDown = provinces;
            List<Location> districts = db.Locations.GroupBy(model => model.District).Select(model => model.FirstOrDefault()).ToList();
            ViewBag.ListofDistricts = districts;
            return View();


        }
        public JsonResult GetDistricts(string district)
        {
            var province = db.Locations.Where(l => l.District == district).First().Province;
            var locations = db.Locations.Where(model => model.Province == province).Distinct().ToList();
            ViewBag.ListofDistricts = locations;
            string data = "";
            foreach (var location in locations) {
                data = data + "<option value='" + location.Province.Distinct() + "'>" + location.District + "</option>";
            }
            db.Configuration.ProxyCreationEnabled = false;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllLocation(string province, string district)
        {
            var v = db.Locations.Where(a => a.Province == province).Where(a => a.District == district).OrderBy(a => a.Title).ToList();
            return new JsonResult { Data = v, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        

        public JsonResult GetMarkerInfo(int locationID)
        {
            Location l = null;
            l = db.Locations.Where(a => a.LocationID.Equals(locationID)).FirstOrDefault();
            return new JsonResult { Data = l, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Locations
        public ActionResult Index(int? page)
        {
            return View(db.Locations.ToList().ToPagedList(page ?? 1, 3));
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationID,Title,Lat,Long,Province,District,Address,ImagePath")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LocationID,Title,Lat,Long,Province,District,Address,ImagePath")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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
