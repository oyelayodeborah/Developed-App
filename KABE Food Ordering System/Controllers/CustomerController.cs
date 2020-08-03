using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Models;
using KABE_Food_Ordering_System.Logic;
namespace KABE_Food_Ordering_System.Controllers
{
    public class CustomerController : Controller
    {
        CustomerLogic customerLogic = new CustomerLogic();
        public BaseLogic<User> userLogic = new BaseLogic<User>(new ApplicationDbContext());
        public BaseLogic<Customer> baseLogic = new BaseLogic<Customer>(new ApplicationDbContext());
        public BaseLogic<Location> locationLogic = new BaseLogic<Location>(new ApplicationDbContext());

        // GET: CustomerProfile
        [AdminRoleRestrictLogic]
        public ActionResult Index()
        {
            return View(baseLogic.GetAll());
        }
        //CheckRecommendedFood
        [CustomerRestrictLogic]
        public ActionResult CheckRecommendedFood()
        {
            int id = Convert.ToInt32(Session["id"]);
            User user = new UserLogic().Get(id);
            var getRecommendedFood = new List<Food>();
            var getRecommendedFoodOne = new CustomerLogic().SetRecommendedFoodOne(id, user.LastLoggedIn);
            if (getRecommendedFoodOne != null)
            {
                var getRecommendedFoodTwo = new CustomerLogic().SetRecommendedFoodTwo(id, user.LastLoggedIn);
                var getRecommendedFoodThree = new CustomerLogic().SetRecommendedFoodThree(id, user.LastLoggedIn);
                getRecommendedFood.Add(new Food
                {
                    Id = getRecommendedFoodOne.Id,
                    Image = getRecommendedFoodOne.Image,
                    ContentType = getRecommendedFoodOne.ContentType,
                    Name = getRecommendedFoodOne.Name
                });
                getRecommendedFood.Add(new Food
                {
                    Id = getRecommendedFoodTwo.Id,
                    Image = getRecommendedFoodTwo.Image,
                    ContentType = getRecommendedFoodTwo.ContentType,
                    Name = getRecommendedFoodTwo.Name
                });
                getRecommendedFood.Add(new Food
                {
                    Id = getRecommendedFoodThree.Id,
                    Image = getRecommendedFoodThree.Image,
                    ContentType = getRecommendedFoodThree.ContentType,
                    Name = getRecommendedFoodThree.Name
                });
            }
            if (getRecommendedFood.Count() == 0)
            {
                ViewBag.Results = null;
            }
            else
            {
                ViewBag.Results = getRecommendedFood;
            }
            return View();
        }
        //GET: User/Create
        [CustomerRestrictLogic]
        public ActionResult Register()
        {
            string status= Convert.ToString(Session["Status"]);
            string setStatus = Convert.ToString(Status.ProfileNotCreated);
            ApplicationDbContext _context = new ApplicationDbContext();
            if (status == setStatus )
            {
                Customer customer = customerLogic.GetCustomerByEmail(Convert.ToString(Session["Email"]));
                if (customer == null)
                {
                    ViewBag.Message = "Error";
                }
                var getCustomer = new CustomerLogic().Get(customer.Id);
                var model = new Customer()
                {
                    Locations = _context.Locations.ToList(),
                    Id = customer.Id,
                    RecommendedFood=getCustomer.RecommendedFood, ResidentialAddress=getCustomer.ResidentialAddress,
                    BestFood = getCustomer.BestFood,
                    DateOfBirth = getCustomer.DateOfBirth,
                    FoodAllergies = getCustomer.FoodAllergies,
                    Occupation = getCustomer.Occupation,
                    PhoneNumber=getCustomer.PhoneNumber,
                    Gender = getCustomer.Gender,
                    LocationId = getCustomer.LocationId,
                    UsersId = getCustomer.UsersId,
                    Name = getCustomer.Name

                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Update", "Customer");
            }


        }

        //POST: User/Create
        [CustomerRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Name,LocationId,ResidentialAddress,PhoneNumber,DateOfBirth,Gender,BestFood," +
            "FoodAllergies,RecommendedFood, Occupation,UsersId")] Customer model)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            model.Locations = _context.Locations.ToList();
            string Email = Convert.ToString(Session["Email"]);
            int id = Convert.ToInt32(Session["id"]);
            Customer custLogic = customerLogic.GetByUserID(id);
            if (custLogic != null /*&& customerLogic.IsDetailsExist(model.Name)*/)
            {
                custLogic.LocationId = model.LocationId;
                custLogic.ResidentialAddress = model.ResidentialAddress;
                custLogic.PhoneNumber = model.PhoneNumber;
                custLogic.FoodAllergies = model.FoodAllergies;
                custLogic.Occupation = model.Occupation;
                custLogic.DateOfBirth = model.DateOfBirth;
                custLogic.Gender = model.Gender;
                custLogic.BestFood = model.BestFood;

                baseLogic.Update(custLogic);
                User user = userLogic.Get(custLogic.UsersId);
                if (user.Status == Status.ProfileNotCreated)
                {
                    user.Status = Status.ChangePassword;
                    userLogic.Update(user);
                }
                //user.SecretQuestions = model.Users.SecretQuestions;

                ViewBag.Message = "Success";

                return RedirectToAction("ChangePassword", "User");
                //return View(model);

            }
            else
            {
                ViewBag.Message = "Exist";
                return View(model);
            }


        }


        //GET: User/Create
        [CustomerRestrictLogic]
        public ActionResult Update()
        {
            int id = Convert.ToInt32(Session["id"]);
            ApplicationDbContext _context = new ApplicationDbContext();
            //if (status == setStatus )
            //{
            
            var getCustomer = new CustomerLogic().GetByUserID(id);
            var model = new Customer()
            {
                Locations = _context.Locations.ToList(),
                Id = id,
                RecommendedFood = getCustomer.RecommendedFood,
                ResidentialAddress = getCustomer.ResidentialAddress,
                BestFood = getCustomer.BestFood,
                DateOfBirth = getCustomer.DateOfBirth,
                FoodAllergies = getCustomer.FoodAllergies,
                Occupation = getCustomer.Occupation,
                PhoneNumber = getCustomer.PhoneNumber,
                Gender = getCustomer.Gender,
                LocationId = getCustomer.LocationId,
                UsersId = getCustomer.UsersId,
                Name = getCustomer.Name

            };
            return View(model);
            
        }

        //POST: User/Create
        [CustomerRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "Id,Name,LocationId,ResidentialAddress,PhoneNumber,DateOfBirth,Gender,BestFood," +
            "FoodAllergies,RecommendedFood, Occupation,UsersId")] Customer model)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            model.Locations = _context.Locations.ToList();
            int id = Convert.ToInt32(Session["id"]);
            Customer custLogic = customerLogic.GetByUserID(id);
            if (custLogic != null )
            {
                custLogic.LocationId = model.LocationId;
                custLogic.ResidentialAddress = model.ResidentialAddress;
                custLogic.PhoneNumber = model.PhoneNumber;
                custLogic.FoodAllergies = model.FoodAllergies;
                var date = Convert.ToDateTime(model.DateOfBirth);
                custLogic.DateOfBirth = date;
                custLogic.Gender = model.Gender;
                custLogic.BestFood = model.BestFood;

                baseLogic.Update(custLogic);
                

                ViewBag.Message = "Success";

                return RedirectToAction("ViewProfile", "Customer");

            }
            else
            {
                ViewBag.Message = "Exist";
                return View(model);
            }


        }

        //GET: Customer/ViewProfile/{id}
        [CustomerRestrictLogic]
        public ActionResult ViewProfile()
        {
            int id = Convert.ToInt32(Session["id"]);
            if (id == 0)
            {
                return HttpNotFound();
            }
            Customer customer = customerLogic.GetByUserID(id);

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //GET: Customer/Details/{id}
        [CustomerRestrictLogic]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Customer customer = baseLogic.Get(id);

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
    }
}