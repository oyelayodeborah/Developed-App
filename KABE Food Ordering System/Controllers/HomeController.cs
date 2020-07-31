using System.Web.Mvc;
using KABE_Food_Ordering_System.Logic;
using KABE_Food_Ordering_System.Models;
using System;

namespace KABE_Food_Ordering_System.Controllers
{
    public class HomeController : Controller
    {

        public BaseLogic<User> baseLogic = new BaseLogic<User>(new ApplicationDbContext());
        public BaseLogic<Restaurant> restaurantLogic = new BaseLogic<Restaurant>(new ApplicationDbContext());
        public BaseLogic<Customer> customerLogic = new BaseLogic<Customer>(new ApplicationDbContext());
        public BaseLogic<Role> roleLogic = new BaseLogic<Role>(new ApplicationDbContext());

        [HttpGet]
        //[SessionRestrictLogic]
        public ActionResult Index()
        {
            var id = Convert.ToInt32(Session["id"]);
            var getRole = Session["role"].ToString();
            var celebrateDate = DateTime.Now;
            var datenow = DateTime.Now.Date;
            if (getRole == "Restaurant")
            {
                var getUserDetails = new RestaurantLogic().GetByUserID(id);
                celebrateDate = getUserDetails.EstablishmentDate.Date;
                if (datenow == celebrateDate)
                {
                    Session["celebrate"] = "We are wishing you a happy anniversary, Looking forward to doing more business with you.";
                }
                else
                {
                    Session["celebrate"] = "";
                }
            }
            else if (getRole == "Customer")
            {
                var getDetails = new CustomerLogic().GetByUserID(id);
                celebrateDate = getDetails.DateOfBirth.Date;
                if (datenow == celebrateDate)
                {
                    Session["celebrate"] = "We are wishing you a happy birthday, Long Life and Prosperity.";
                }
                else
                {
                    Session["celebrate"] = "";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var Id = Convert.ToInt32(Session["Id"]);
            //User user = baseLogic.Get(Id);
            //user.LastLoggedOut = DateTime.Now;
            //baseLogic.Update(user);
            if (Id > 0)
            {
                User updateUser = baseLogic.Get(Id);
                updateUser.LastLoggedIn = DateTime.Now;
                if (updateUser.Status==Status.Active /*!= Status.ProfileNotCreated || updateUser.Status != Status.ChangePassword*/)
                {
                    updateUser.Status = Status.InActive;
                }
                baseLogic.Update(updateUser);

                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
