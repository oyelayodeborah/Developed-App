using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Models;
using KABE_Food_Ordering_System.Logic;
using KABE_Food_Ordering_System.ViewModels;
using System.Web.Helpers;
using System.Data.Entity.Validation;

namespace KABE_Food_Ordering_System.Controllers
{
    public class UserController : Controller
    {
        public BaseLogic<User> baseLogic = new BaseLogic<User>(new ApplicationDbContext());
        public BaseLogic<Restaurant> restaurantLogic = new BaseLogic<Restaurant>(new ApplicationDbContext());
        public BaseLogic<Customer> customerLogic = new BaseLogic<Customer>(new ApplicationDbContext());
        public BaseLogic<Role> roleLogic = new BaseLogic<Role>(new ApplicationDbContext());

        public AccountLogic accountLogic = new AccountLogic();


        // GET: User
        public ActionResult Index()
        {
            return View(baseLogic.GetAll());
        }
        [SessionRestrictLogic]
        public ActionResult ViewProfile()
        {

            return View();
        }
        // GET: User/ChangePassword
        [SessionRestrictLogic]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //POST: User/ChangePassword
        [SessionRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangeUserPasswordViewModels model)
        {

            try
            {
                model.Email = Session["Email"].ToString();
                ApplicationDbContext _context = new ApplicationDbContext();
                var password = Convert.ToString(Crypto.Hash(model.CurrentPassword));
                var findCurrentPassword = _context.Users.Where(c => c.Email == model.Email);
                if (findCurrentPassword != null)
                {
                    var getUser = findCurrentPassword.SingleOrDefault();
                    User user = baseLogic.Get(getUser.Id);
                    user.Password = Crypto.Hash(model.NewPassword);
                    user.Status = Status.Active;
                    Session["Status"] = user.Status.ToString();
                    var getRole = roleLogic.Get(getUser.RoleId);
                    accountLogic.NotifyEmail(getUser.Email, getRole.Name);
                    baseLogic.Update(user);
                    ViewBag.Message = "Success";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewBag.Message = "Incorrect Password";
                    return View(model);
                }
                
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", ex.ToString());
            //    return View(model);
            //}

        }

    }
}