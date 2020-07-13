using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Logic;
using KABE_Food_Ordering_System.Models;
using KABE_Food_Ordering_System.ViewModels;
//using CMS.DataEngine;

//using Kentico.Search;

namespace KABE_Food_Ordering_System.Controllers
{
    public class OrderController : Controller
    {
        FoodLogic food = new FoodLogic();
        LocationLogic location = new LocationLogic();
        RestaurantLogic restaurant = new RestaurantLogic();

        public BaseLogic<Food> foodLogic = new BaseLogic<Food>(new ApplicationDbContext());
        public BaseLogic<Order> orderLogic = new BaseLogic<Order>(new ApplicationDbContext());
        public BaseLogic<Restaurant> restaurantLogic = new BaseLogic<Restaurant>(new ApplicationDbContext());
        public BaseLogic<Location> locationLogic = new BaseLogic<Location>(new ApplicationDbContext());



        [CustomerRestrictLogic]
        [HttpGet]
        public ActionResult New(int foodId, string restaurant, decimal price)
        {
            FoodViewModels foodViewModels = new FoodViewModels();
            foodViewModels.Id = foodId;
            var findFood = new FoodLogic().Get(foodId);
            Order order = new Order();

            if (findFood != null)
            {
                foodViewModels.Name = findFood.Name;
                foodViewModels.RestaurantName = restaurant;
                foodViewModels.ContentType = findFood.ContentType;
                foodViewModels.Image = findFood.Image;
                foodViewModels.Price = price;
                ViewBag.Data = foodViewModels;
                order.CustomerId = Convert.ToInt32(Session["id"]);
                order.FoodId = foodViewModels.Id;
                order.Amount = Convert.ToString(price);
                order.RestaurantId = new RestaurantLogic().FindByName(foodViewModels.RestaurantName).Id;
                order.TransactionReference = generateGuid();
                order.Locations = locationLogic.GetAll();
            }
            
            return View(order);
        }
        

        //POST: Order/New
        [HttpPost]
        [CustomerRestrictLogic]
        public ActionResult New(Order order)
        {
            order.Locations = locationLogic.GetAll();

            if (order.Amount != "0")
            {
                order.CardNumber = "00";
                order.DeliveryDateTime = DateTime.Now;
                order.OrderDateTime = DateTime.Now;
                order.Status = OrderStatus.NotPaid;
                order.VerificationCode = GenerateVerfCode();
                var email = Session["email"].ToString();
                var name = Session["username"].ToString();
                new AccountLogic().SendingVerificationCode(email, name, order.VerificationCode);
                var rest = new RestaurantLogic().Get(order.RestaurantId);
                order.RestaurantId = rest.RestaurantsId;
                orderLogic.Save(order);
                var price = Convert.ToInt32(order.Amount) / Convert.ToInt32(order.FoodQuantity);
                Session["OrderId"] = order.Id;
                ViewBag.Amount = order.Amount;
                ViewBag.VefriCode = order.VerificationCode;
                return View("Payment");
            }
            else
            {
                return View(order);
            }
            //else
            //{
            //    ViewBag.NoAmount = true;
            //    return Redirect("http://localhost:50125/Order/New?foodId=" + order.FoodId + "&restaurant=" + rest.Name + "&price=" + price);
            //}
        }
        [CustomerRestrictLogic]
        public ViewResult Search(string searchTerm)
        {
            if (searchTerm != null)
            {
                var Food = new RestaurantLogic().GetListRestaurantFoods(searchTerm);
                int i = 0;
                var getRestaurant = new List<FoodViewModels>();
                var getLocation = new List<FoodViewModels>();
                var getFood = new List<FoodViewModels>();
                if (Food.Count() == 0)
                {
                    var restaurant = new RestaurantLogic().GetFoodsByRestaurant(searchTerm);

                    if (restaurant.Count != 0)
                    {
                        var getPrice = new RestaurantLogic().GetListRestaurantFoodsPrice(searchTerm).ToArray();

                        ViewBag.Results = getRestaurant;
                        ViewBag.TotalItems = getRestaurant.Count();
                    }
                    else
                    {
                        var Location = new RestaurantLogic().GetFoodsByLocation(searchTerm);
                        if (Location.Count != 0)
                        {
                            var getPrice = new RestaurantLogic().GetListRestaurantFoodsPrice(searchTerm).ToArray();
                            ViewBag.Results = getLocation;
                            ViewBag.TotalItems = getLocation.Count();
                        }
                        else
                        {

                            ViewBag.Results = null;
                            ViewBag.TotalItems = 0;

                        }
                    }
                }
                else
                {
                    var getPrice = new RestaurantLogic().GetListRestaurantFoodsPrice(searchTerm).ToArray();

                    getFood = new RestaurantLogic().GetListRestaurantFoods(searchTerm);
                    ViewBag.Results = getFood;
                    ViewBag.TotalItems = getFood.Count();

                }

            }
            else
            {
                var getAll = new RestaurantLogic().GetAllRestaurantFoodsList();
                var getAllFoods = new List<FoodViewModels>();

                ViewBag.Results = getAll;
                ViewBag.TotalItems = getAll.Count();

            }

            return View();
        }
        // GET: Order/Payment
        [HttpGet]
        [OrderProcessing]
        [CustomerRestrictLogic]
        public ActionResult Payment()
        {
            int id= Convert.ToInt32(Session["OrderId"]);
            Order order = new OrderLogic().Get(id);
            ViewBag.Amount = order.Amount;
            if (order.Status == OrderStatus.NotPaid)
            {
                return View(order);
            }
            else
            {
                return RedirectToAction("Search");
            }

        }

        [HttpPost]
        [OrderProcessing]
        [CustomerRestrictLogic]
        public ActionResult Payment(Order order)
        {
            var cardNumber = Request.Form["cardNumber"];
            var pin = Request.Form["pin"];
            var expiryDateMonth = Request.Form["expiryDateMonth"];
            var expiryDateYear = Request.Form["expiryDateYear"];
            var verificationCode = Request.Form["verificationCode"];
            var orderId = Convert.ToInt32(Session["OrderId"]);
            var getOrder = new OrderLogic().Get(orderId);
            getOrder.CardNumber = cardNumber;
            getOrder.Status = OrderStatus.Pending;
            var deliveryTime = getOrder.DeliveryDateTime.AddHours(2);
            getOrder.DeliveryDateTime = deliveryTime;
            verificationCode = getOrder.VerificationCode;
            if (getOrder.VerificationCode == verificationCode)
            {
                orderLogic.Update(getOrder);
                ViewBag.Message = "Success";
                return RedirectToAction("All");
            }
            else
            {
                ViewBag.Message = "Error";
                return RedirectToAction("All");
            }

        }
        // GET: Order
        [HttpGet]
        [AdminRoleRestrictLogic]
        public ActionResult Index()
        {
            var id = Convert.ToInt32(Session["id"]);
            var getRestaurant = new RestaurantLogic().GetByUserID(id);
            if (getRestaurant != null)
            {

                return View(orderLogic.GetAll());

            }
            else
            {
                return View(orderLogic.GetAll());

            }
        }

        [HttpGet]
        [CustomerRestrictLogic]
        public ActionResult All()
        {
            var id = Convert.ToInt32(Session["id"]);
            var getOrder = new OrderLogic().GetCustomerOrders(id);
            return View(getOrder);
        }

        [HttpGet]
        [RestaurantRestrictLogic]
        public ActionResult Deliver(int id)
        {
            Session["OrderId"] = id;
            return RedirectToAction("Delivered");
        }
        [HttpGet]
        [CustomerRestrictLogic]
        public ActionResult Cancel(int id)
        {
            Session["OrderId"] = id;
            return RedirectToAction("Cancelled");
        }
        [HttpGet]
        [CustomerRestrictLogic]
        public ActionResult MakePayment(int id)
        {
            Session["OrderId"] = id;
            return RedirectToAction("Payment");
        }
        [HttpGet]
        [RestaurantRestrictLogic]
        public ActionResult Delivered()
        {
            var id = Convert.ToInt32(Session["id"]);
            var Orderid = Convert.ToInt32(Session["OrderId"]);
            if (Orderid != 0)
            {
                var getOrder = new OrderLogic().Get(Orderid);
                getOrder.Status = OrderStatus.Delivered;
                orderLogic.Update(getOrder);
                return RedirectToAction("ViewAll");
            }
            else
            {
                return RedirectToAction("ViewAll");
            }
            
        }
        [HttpGet]
        [CustomerRestrictLogic]
        public ActionResult Cancelled()
        {
            var id = Convert.ToInt32(Session["id"]);
            var Orderid = Convert.ToInt32(Session["OrderId"]);
            if (Orderid != 0)
            {
                var getOrder = new OrderLogic().Get(Orderid);
                getOrder.Status = OrderStatus.Cancelled;
                orderLogic.Update(getOrder);
                return RedirectToAction("All");
            }
            else
            {
                return RedirectToAction("All");
            }

        }

        
        // GET: Order/ViewAll
        [HttpGet]
        [RestaurantRestrictLogic]
        public ActionResult ViewAll()
        {
            var id = Convert.ToInt32(Session["id"]);
            var getOrder = new OrderLogic().GetRestaurantOrders(id);
            return View(getOrder);
        }
        public string generateGuid()
        {
            string cryptedValue = Guid.NewGuid().ToString("N");
            return cryptedValue;
        }
        public string GenerateVerfCode()
        {
            var codeLength = 6;
            string allowedChars = "0123456789";
            Random randChars = new Random();
            char[] chars = new char[codeLength];
            int allowedCharC = allowedChars.Length;
            for (int i = 0; i < codeLength; i++)
            {
                chars[i] = allowedChars[(int)((allowedChars.Length) * randChars.NextDouble())];
            }
            var password = new string(chars);
            return password.ToString();
        }

    }
}