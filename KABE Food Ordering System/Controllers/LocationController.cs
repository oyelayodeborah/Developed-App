using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Logic;
using KABE_Food_Ordering_System.Models;

namespace KABE_Food_Ordering_System.Controllers
{
    public class LocationController : Controller
    {
        public LocationLogic locationLogic = new LocationLogic();
        public BaseLogic<Location> baseLogic = new BaseLogic<Location>(new ApplicationDbContext());

        // GET: Role
        public ActionResult Index()
        {
            return View(baseLogic.GetAll());
        }

        // GET: Location/New ---Returns the Location view page
        public ActionResult New()
        {
            return View();
        }
        // POST: Location/New ----Post the data gotten from the location
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(/*[Bind(Include = "Name")]*/  Location model)
        {
                try
                {
                if (locationLogic.IsDetailsExist(model.Name))
                {
                    baseLogic.Save(model);
                    ViewBag.Message = "Success";

                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Exist";
                    return View(model);

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return View(model);
            }
        }
    }
}