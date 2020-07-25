using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Models;
using KABE_Food_Ordering_System.Logic;
using System.Web.Helpers;
using System.Data.Entity.Validation;

namespace KABE_Food_Ordering_System.Controllers
{
    public class AccountController : Controller
    {
        public AccountLogic accountLogic = new AccountLogic();

        public BaseLogic<User> baseLogic = new BaseLogic<User>(new ApplicationDbContext());
        public BaseLogic<Restaurant> restaurantLogic = new BaseLogic<Restaurant>(new ApplicationDbContext());
        public BaseLogic<Customer> customerLogic = new BaseLogic<Customer>(new ApplicationDbContext());
        public BaseLogic<Role> roleLogic = new BaseLogic<Role>(new ApplicationDbContext());


        public BaseLogic<Location> locationLogic = new BaseLogic<Location>(new ApplicationDbContext());





        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        // GET: Account/SendCredential
        [AdminRoleRestrictLogic]
        public ActionResult SendCredential()
        {
            var roles = roleLogic.GetAll();
            List<Role> role = new List<Role>();
            foreach (var item in roles)
            {
                if (item.Name != "Admin")
                {
                    role.Add(new Role { Id = item.Id, Name = item.Name });
                }
            }
            var model = new User()
            {
                Roles = role
            };
            return View(model);
        }

        //POST: Account/SendCredential
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendCredential([Bind(Include = "Email,RoleId,Name")]  User model)
        {
            var roles = roleLogic.GetAll();
            List<Role> role = new List<Role>();
            foreach (var item in roles)
            {
                if (item.Name != "Admin")
                {
                    role.Add(new Role { Id = item.Id, Name = item.Name });
                }
            }
            model.Roles = role;

            try
            {
                if (accountLogic.IsDetailsExist(model.Email) && accountLogic.IsNameDetailsExist(model.Name))
                {
                    RoleLogic roleLogic = new RoleLogic();
                    Role getRole = roleLogic.roleRepo.Get(model.RoleId);
                    string password = accountLogic.GeneratePassword();
                    password = "password";
                    string hashedPassword = Crypto.Hash(password);
                    model.Password = hashedPassword;
                    model.LastLoggedIn = DateTime.Now;
                    model.LastLoggedOut = DateTime.Now;
                    model.SecretQuestions = "?";
                    model.DateCreated = DateTime.Now;


                    if (getRole.Name == "Administrator")
                    {
                        model.Status = Status.InActive;
                        accountLogic.SendingEmail(model.Email, getRole.Name, password);
                        baseLogic.Save(model);

                    }
                    else
                    {
                        model.Status = Status.ProfileNotCreated;
                        baseLogic.Save(model);
                        User user = baseLogic.Find(c => c.Email == model.Email).SingleOrDefault();
                        int id = user.Id;
                        if (getRole.Name.Contains("Customer"))
                        {
                            accountLogic.SaveCustomer(user);
                            accountLogic.SendingEmail(user.Email, getRole.Name, password);
                        }
                        else if (getRole.Name.Contains("Restaurant"))
                        {
                            accountLogic.SaveRestaurant(user);
                            accountLogic.SendingEmail(user.Email, getRole.Name, password);

                        }
                    }
                    ViewBag.Message = "Success";
                    //return RedirectToAction("Index","Home");
                    //return RedirectToAction("SendCredential", "Account");
                    return View("Index", "Home");

                }
                else
                {
                    ViewBag.Message = "Exist";
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


        // GET: Account/Login

        public ActionResult Login(int? id)
        {
            var user = baseLogic.GetAll();
            if (user.Count() == 0)
            {
                return RedirectToAction("Register", "Account");
            }
            else
            {
                return View();
            }
        }


        //POST: Account/Login
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Login()
        {
            User user = new User();
            user.Email = Request.Form["Email"];
            user.Password = Request.Unvalidated["Password"];
            ApplicationDbContext _context = new ApplicationDbContext();
            ViewBag.Message = "";
            if (Session["id"] == null)
            {
                if (user == null)
                {
                    return View("Login");
                }
                var getEmail = (user.Email).ToLower().Trim();
                var getPassword = user.Password;
                var hashedPassword = Crypto.Hash(user.Password.Trim());
                var findEmailAndPassword = _context.Users.Where(a => a.Password == hashedPassword && a.Email == getEmail).FirstOrDefault();
                if (findEmailAndPassword != null)
                {
                    //Setting the user session
                    Session["id"] = findEmailAndPassword.Id;
                    Session["password"] = getPassword;
                    Session["email"] = findEmailAndPassword.Email;
                    Session["username"] = findEmailAndPassword.Name;
                    var getRole = roleLogic.Get(findEmailAndPassword.RoleId); ;
                    Session["role"] = getRole.Name;
                    Session["Status"] = findEmailAndPassword.Status.ToString();
                    if (getRole.Name == "Restaurant")
                    {
                        var getUserDetails = new RestaurantLogic().GetByUserID(findEmailAndPassword.Id);
                        Session["celebrate"] = accountLogic.Celebrate(getRole.Name, findEmailAndPassword.Email, findEmailAndPassword.Name, getUserDetails.EstablishmentDate, findEmailAndPassword.LastLoggedIn);
                    }
                    else if (getRole.Name == "Customer")
                    {
                        var getDetails = new CustomerLogic().GetByUserID(findEmailAndPassword.Id);
                        Session["celebrate"] = accountLogic.Celebrate(getRole.Name, findEmailAndPassword.Email, findEmailAndPassword.Name, getDetails.DateOfBirth, findEmailAndPassword.LastLoggedIn);
                    }

                    //Updating the lastLoggedIn details
                    User updateUser = baseLogic.Get(findEmailAndPassword.Id);
                    updateUser.LastLoggedIn = DateTime.Now;

                    if (findEmailAndPassword.Status == Status.InActive/*!= Status.ProfileNotCreated && findEmailAndPassword.Status != Status.ChangePassword*/)
                    {
                        updateUser.Status = Status.Active;

                        baseLogic.Update(updateUser);

                        return RedirectToAction("Index", "Home");
                    }


                    //check if the user has created profile or not
                    if (findEmailAndPassword.Status == Status.ProfileNotCreated && getRole.Name.Contains("Restaurant"))
                    {
                        baseLogic.Update(updateUser);

                        return RedirectToAction("Register", "Restaurant");
                    }
                    else if (findEmailAndPassword.Status == Status.ProfileNotCreated && getRole.Name.Contains("Customer"))
                    {
                        baseLogic.Update(updateUser);

                        return RedirectToAction("Register", "Customer");
                    }
                    else if (findEmailAndPassword.Status == Status.ChangePassword)
                    {
                        baseLogic.Update(updateUser);

                        return RedirectToAction("ChangePassword", "User");

                    }
                    else if (findEmailAndPassword.Status == Status.Deactivated)
                    {
                        return RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        updateUser.LastLoggedIn = DateTime.Now;

                        baseLogic.Update(updateUser);

                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewBag.Message = "Incorrect login details";
                return View(user);
            }
            ViewBag.Message = "Session currently Exist";
            return RedirectToAction("Index", "Home");
        }



        // GET: Account/SendCredential
        public ActionResult Register()
        {
            var user = baseLogic.GetAll();
            if (user.Count() == 0)
            {
                ApplicationDbContext _context = new ApplicationDbContext();
                var role = _context.Roles.ToList();

                if (role.Count == 0)
                {
                    var newRole = new Role();
                    newRole.Name = "Admin";
                    roleLogic.Save(newRole);
                }
                var model = new User()
                {
                    Roles = role//roleLogic.GetAll()
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //POST: Account/SendCredential
        [NoUserOnlyRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Email,RoleId,Name")]  User model)
        {
            RoleLogic roleLogic = new RoleLogic();

            model.Roles = roleLogic.GetAll();
            Role getRole = roleLogic.roleRepo.Get(model.RoleId);
            string password = accountLogic.GeneratePassword();
            password = "password";
            model.Password = Crypto.Hash(password);
            //accountLogic.SendingEmail(model.Email, getRole.Name, password);
            model.LastLoggedIn = DateTime.Now;
            model.LastLoggedOut = DateTime.Now;
            model.Status = Status.InActive;
            model.SecretQuestions = "?";
            model.DateCreated = DateTime.Now;

            try     //if success
            {
                var sendMail = /*"Successful";*/ accountLogic.SendingMail(model.Email, getRole.Name, password);
                var mail = sendMail;
                //sendMail = "Successful";
                //if (sendMail == "Successful")
                //    {
                baseLogic.Save(model);



                ViewBag.Message = "Success";

                return RedirectToAction("Login", "Account");
                //}
                //else
                //{
                //    TempData["Message"] = "Email error";
                //    return View(model);
                //}

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                TempData["Message"] = "Error";
                return View(model);
            }
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", ex.ToString());
            //    return View(model);
            //}
        }

        // GET: Account/SendCredential
        [AdminRoleRestrictLogic]
        public ActionResult ChangeUserPassword()
        {
            var model = new User()
            {
                Roles = roleLogic.GetAll()
            };
            return View(model);
        }

        //POST: Account/SendCredential
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserPassword([Bind(Include = "Email,Name"/*,RoleId"*/)]  User model)
        {
            model.Roles = roleLogic.GetAll();

            try
            {
                if (!(accountLogic.IsDetailsExist(model.Email) && accountLogic.IsNameDetailsExist(model.Name)))
                {
                    ApplicationDbContext _context = new ApplicationDbContext();
                    var getUser = _context.Users.Where(c => c.Email == model.Email);
                    if (getUser.Count() == 1)
                    {
                        var getuser = _context.Users.Where(c => c.Email == model.Email).SingleOrDefault();
                        var getRole = roleLogic.Get(getuser.RoleId);


                        User updatePassword = baseLogic.Get(getuser.Id);
                        string password = accountLogic.GeneratePassword();
                        password = "password";
                        string hashedPassword = Crypto.Hash(password);
                        model.Password = hashedPassword;

                        model.Status = Status.InActive;
                        accountLogic.SendingMail(model.Email, getRole.Name, password);
                        baseLogic.Update(updatePassword);
                        ViewBag.Message = "Success";
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ViewBag.Message = "Multiple";
                        return View(model);
                    }

                }

                else
                {
                    ViewBag.Message = "Dont Exist";
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