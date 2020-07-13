using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Models;
using KABE_Food_Ordering_System.Logic;


namespace KABE_Food_Ordering_System.Controllers
{
    public class RoleController : Controller
    {
        public RoleLogic roleLogic = new RoleLogic();
        public BaseLogic<Role> baseLogic = new BaseLogic<Role>(new ApplicationDbContext());

        // GET: Role
        public ActionResult Index()
        {
            return View(baseLogic.GetAll());
        }


        // GET: Role/New ---Returns the Role view page
        public ActionResult New()
        {
            return View();
        }
        // POST: Role/New ----Post the data gotten from the user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(/*[Bind(Include = "Name")]*/  Role model)
        {
            //if (ModelState.IsValid)
            //{
                //try
                //{
                    if (roleLogic.IsDetailsExist(model.Name))
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
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", ex.ToString());
                //    return View(model);
                //}
            //}
            //return View(model);
        }
    }
}