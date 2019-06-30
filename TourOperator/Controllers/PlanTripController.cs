using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourOperator.Models;

namespace TourOperator.Controllers
{
    public class PlanTripController : Controller
    {
        private DreamTourContext db = new DreamTourContext();
        public ActionResult Index()
        {
            List<Package> Package = db.Packages.GroupBy(model => model.Status).Select(model => model.FirstOrDefault()).ToList();
            return View(Package);
        }
    }
}