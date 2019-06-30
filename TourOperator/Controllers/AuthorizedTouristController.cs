using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourOperator.Models;

namespace TourOperator.Controllers
{
    public class AuthorizedTouristController : Controller
    {
        private DreamTourContext db = new DreamTourContext();



        [Authorize]
        public ActionResult Index(string name)
        {
            db.Tourists.Where(model => model.FirstName == name);
            return View();
        }
    }
}