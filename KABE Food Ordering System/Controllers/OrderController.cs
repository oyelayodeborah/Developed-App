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
        public ActionResult Pay(int id)
        {
            Order order = new OrderLogic().Get(id);
            FoodViewModels foodViewModels = new FoodViewModels();
            var findFood = new Food();
            var restaurant = new Restaurant();
            if (order.Status == OrderStatus.NotPaid)
            {
                findFood = new FoodLogic().Get(order.FoodId);
                restaurant = new RestaurantLogic().GetByUserID(order.RestaurantId);
                foodViewModels.Id = order.FoodId;

                foodViewModels.Name = findFood.Name;
                foodViewModels.RestaurantName = restaurant.Name;
                foodViewModels.ContentType = findFood.ContentType;
                foodViewModels.Image = findFood.Image;
                if (order.FoodQuantity > 0)
                {
                    foodViewModels.Price = Convert.ToDecimal(order.Amount) / Convert.ToInt32(order.FoodQuantity);
                }
                else
                {
                    foodViewModels.Price = Convert.ToDecimal(order.Amount) / 1;
                }
                var location = new LocationLogic().Get(restaurant.LocationId);
                foodViewModels.Location = location.Name;
                foodViewModels.About = restaurant.About;
                ViewBag.Data = foodViewModels;
                ViewBag.Msg = 1;
                ViewBag.VefriCode = order.VerificationCode;
                order.Locations = new LocationLogic().GetAll();
                return View(order);
            }
            else
            {
                return RedirectToAction("All");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomerRestrictLogic]
        public ActionResult Pay(Order getorder)
        {
            Order order = new OrderLogic().Get(getorder.Id);
            order.Locations = locationLogic.GetAll();
            order.ResidentialAddress = getorder.ResidentialAddress;
            order.Amount = getorder.Amount;
            order.FoodQuantity = getorder.FoodQuantity;
            order.LocationId = getorder.LocationId;
            if (order.Amount != "0")
            {
                //order.CardNumber = "00";
                //order.DeliveryDateTime = DateTime.Now;
                //order.OrderDateTime = DateTime.Now;
                //order.Status = OrderStatus.NotPaid;
                //order.VerificationCode = GenerateVerfCode();
                var email = Session["email"].ToString();
                var name = Session["username"].ToString();
                new AccountLogic().SendingVerificationCode(email, name, order.VerificationCode);
                //var rest = new RestaurantLogic().GetByUserID(order.RestaurantId);
                //order.RestaurantId = rest.RestaurantsId;
                //orderLogic.Update(order);
                //var price = Convert.ToInt32(order.Amount) / Convert.ToInt32(order.FoodQuantity);
                Session["OrderId"] = order.Id;
                Session["NewOrderId"] = order.Id;
                ViewBag.Amount = order.Amount;
                ViewBag.VefriCode = order.VerificationCode;
                //return View("Payment");
                ViewBag.Msg = 2;
                return View(order);
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
        [HttpGet]
        public ActionResult New(int foodId, string restaurant, decimal price)
        {
            FoodViewModels foodViewModels = new FoodViewModels();
            foodViewModels.Id = foodId;
            var findFood = new FoodLogic().Get(foodId);
            Order order = new Order();
            order.Locations = locationLogic.GetAll();
            if (findFood != null)
            {
                foodViewModels.Name = findFood.Name;
                foodViewModels.RestaurantName = restaurant;
                foodViewModels.ContentType = findFood.ContentType;
                foodViewModels.Image = findFood.Image;
                foodViewModels.Price = price;
                ViewBag.Data = foodViewModels;
                order.CustomerId = Convert.ToInt32(Session["id"]);
                order.FoodId = foodId;
                order.VerificationCode = GenerateVerfCode();
                ViewBag.VefriCode = order.VerificationCode;
                order.Amount = Convert.ToString(price);
                order.RestaurantId = new RestaurantLogic().FindByName(foodViewModels.RestaurantName).Id;
                order.TransactionReference = generateGuid();
                ViewBag.Msg = 1;
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
                //order.VerificationCode = GenerateVerfCode();
                var email = Session["email"].ToString();
                var name = Session["username"].ToString();
                new AccountLogic().SendingVerificationCode(email, name, order.VerificationCode);
                var rest = new RestaurantLogic().Get(order.RestaurantId);
                order.RestaurantId = rest.RestaurantsId;
                orderLogic.Save(order);
                var price = Convert.ToInt32(order.Amount) / Convert.ToInt32(order.FoodQuantity);
                Session["OrderId"] = order.Id;
                Session["NewOrderId"] = order.Id;
                ViewBag.Amount = order.Amount;
                ViewBag.VefriCode = order.VerificationCode;
                //return View("Payment");
                ViewBag.Msg = 2;
                return View(order);
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
            if (searchTerm != "" && searchTerm != null)
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
                        //var getPrice = new RestaurantLogic().GetListRestaurantFoodsPrice(searchTerm).ToArray();

                        ViewBag.Results = getRestaurant;
                        ViewBag.TotalItems = getRestaurant.Count();
                    }
                    else
                    {
                        var Location = new RestaurantLogic().GetFoodsByLocation(searchTerm);
                        if (Location.Count != 0)
                        {
                            //var getPrice = new RestaurantLogic().GetListRestaurantFoodsPrice(searchTerm).ToArray();
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
                    //var getPrice = new RestaurantLogic().GetListRestaurantFoodsPrice(searchTerm).ToArray();

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
        //// GET: Order/Payment
        //[HttpGet]
        //[OrderProcessing]
        //[CustomerRestrictLogic]
        //public ActionResult Payment()
        //{
        //    int id= Convert.ToInt32(Session["NewOrderId"]);
        //    Order order = new OrderLogic().Get(id);
        //    ViewBag.Amount = order.Amount;
        //    if (order.Status == OrderStatus.NotPaid)
        //    {
        //        return View(order);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Search");
        //    }

        //}

        [HttpPost]
        [OrderProcessing]
        [CustomerRestrictLogic]
        //[ValidateAntiForgeryToken]
        public ActionResult Payment()
        {
            int orderId = Convert.ToInt32(Session["NewOrderId"]);
            if (orderId == 0)
            {
                ViewBag.Message = "Error";
                return RedirectToAction("All");
            }
            var getOrder = new OrderLogic().Get(orderId);

            if (getOrder.Status != OrderStatus.NotPaid)
            {
                ViewBag.Message = "Exist";
                return RedirectToAction("All");
            }
            var cardNumber = Request.Form["cardNumber"];
            var pin = Request.Form["pin"];
            var expiryDateMonth = Request.Form["expiryDateMonth"];
            var expiryDateYear = Request.Form["expiryDateYear"];
            var verificationCode = Request.Form["verfCode"];
            getOrder.CardNumber = cardNumber;
            getOrder.Status = OrderStatus.Pending;
            getOrder.OrderDateTime = DateTime.Now;
            var deliveryTime = getOrder.OrderDateTime.AddHours(2);
            getOrder.DeliveryDateTime = deliveryTime;
            var intMonth = 0;
            switch (expiryDateMonth)
            {
                case "January":
                    intMonth = 1;
                    break;
                case "February":
                    intMonth = 2;
                    break;
                case "March":
                    intMonth = 3;
                    break;
                case "April":
                    intMonth = 4;
                    break;
                case "May":
                    intMonth = 5;
                    break;
                case "June":
                    intMonth = 6;
                    break;
                case "July":
                    intMonth = 7;
                    break;
                case "August":
                    intMonth = 8;
                    break;
                case "September":
                    intMonth = 9;
                    break;
                case "October":
                    intMonth = 10;
                    break;
                case "November":
                    intMonth = 11;
                    break;
                case "December":
                    intMonth = 12;
                    break;
                default:
                    break;
            }

            var actualMonth = DateTime.Now.Month;
            //verificationCode = getOrder.VerificationCode;
            //if (intMonth >= actualMonth)
            //{
            //    orderLogic.Update(getOrder);
            //    ViewBag.Message = "Success";
            //    return RedirectToAction("All");
            //}
            if (intMonth < actualMonth)
            {
                //orderLogic.Update(getOrder);
                ViewBag.Message = "Expiry";
                return RedirectToAction("All");
            }
            else if (getOrder.VerificationCode == verificationCode)
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

        //[HttpGet]
        [CustomerRestrictLogic]
        public ActionResult All()
        {
            var id = Convert.ToInt32(Session["id"]);
            var getOrder = new OrderLogic().GetCustomerOrders(id);
            return View(getOrder == null ? new List<Order>() : getOrder);
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