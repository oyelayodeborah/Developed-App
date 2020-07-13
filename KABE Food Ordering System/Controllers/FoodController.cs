using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Logic;
using KABE_Food_Ordering_System.Models;
using KABE_Food_Ordering_System.ViewModels;

namespace KABE_Food_Ordering_System.Controllers
{
    public class FoodController : Controller
    {
        public FoodLogic foodLogic = new FoodLogic();
        public BaseLogic<Food> baseLogic = new BaseLogic<Food>(new ApplicationDbContext());
        public BaseLogic<Restaurant> restaurantLogic = new BaseLogic<Restaurant>(new ApplicationDbContext());


        
        // GET: Food
        [RestaurantRestrictLogic]
        public ActionResult Index()
        {
            RestaurantLogic resLogic = new RestaurantLogic();
            int id = Convert.ToInt32(Session["id"]);

            try
            {
                ApplicationDbContext _context = new ApplicationDbContext();

                //get all food
                var food = resLogic.GetAllRestaurantFoodsList(id);
                if (food != null)
                {
                    ViewBag.Results = food;
                    ViewBag.TotalItems = food.Count();

                    //    //get all price
                    //    var getPrice = resLogic.GetAllRestaurantFoodPrices(id);
                    //    ViewBag.Price = getPrice;
                    //}
                    return View(food);
                }
                //var getFood = new List<string>();
                //var findFood = new Food();
                //if (Food != null)
                //{
                //    foreach (var item in Food)
                //    {
                //        if (item != 0)
                //        {
                //            findFood = baseLogic.Get(item);
                //            getFood.Add(findFood.Name);
                //        }

                //    }
                else
                {
                    ViewBag.Results = null;
                    ViewBag.TotalItems = 0;

                    //    //get all price
                    //    var getPrice = resLogic.GetAllRestaurantFoodPrices(id);
                    //    ViewBag.Price = getPrice;
                    //}
                    return View(new List<Food>());
                }
                    
            }
            catch (Exception)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return PartialView("Error");
            }
            //return View();
        }

        

        // GET: Food/Add ---Returns the Location view page
        [RestaurantRestrictLogic]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(/*[Bind(Include = "Name")]*/ HttpPostedFileBase postedFile, Food model)
        {
            var price = Request.Form["price"];
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var restLogic = new RestaurantLogic();
            model.ContentType = postedFile.ContentType;
            model.Image = Convert.ToBase64String(bytes);
            var Id = Convert.ToInt32(Session["Id"]);
            Restaurant restaurant = new RestaurantLogic().GetByUserID(Id);
            ApplicationDbContext _context = new ApplicationDbContext();
            //if (restaurant != null)
            //{
            //    model.Restaurants.Add(restaurant.Id, price);
            //}
            int id = Convert.ToInt32(Session["id"]);

            var Food = restLogic.GetAllRestaurantFoods(id);
            var IsExist = false;
            
             try
            {
                if (foodLogic.IsDetailsExist(model.Name) /*&& foodLogic.IsRestaurantExist(model.Restaurants, Id)*/)
                {
                    //baseLogic.Save(model);
                    foodLogic.Save(model);
                    Food getFood = new FoodLogic().FindByName(model.Name);
                    var food = restaurant.FoodId;
                    var prices = restaurant.Price;
                    if (food == null)
                    {
                        food = "";
                        restaurant.FoodId = Convert.ToString(getFood.Id) + "," + Convert.ToString(price);

                    }
                    else
                    {
                        restaurant.FoodId = food + "," + Convert.ToString(getFood.Id) + "," + Convert.ToString(price);
                        //restaurant.FoodId = food + "," + Convert.ToString(getFood.Id);
                    }
                    if (prices == null)
                    {
                        prices = "";
                        restaurant.Price = Convert.ToString(price);

                    }
                    else
                    {
                        restaurant.Price = prices + "," + Convert.ToString(price);

                    }
                    //restaurant.FoodId = food + "," + Convert.ToString(getFood.Id);
                    //restaurant.Price = prices + "," + Convert.ToString(price);

                    //restaurant.Foods.Add(Convert.ToString(getFood.Id), price);
                    restaurantLogic.Update(restaurant);

                    ViewBag.Message = "Success";


                    return RedirectToAction("Index");

                }
                else
                {
                    Food getFood = new FoodLogic().FindByName(model.Name);
                    if (Food != null)
                    {
                        foreach (var item in Food)
                        {
                            if (item == getFood.Id)
                            {
                                IsExist = true;
                            }

                        }
                    }
                    if (IsExist == true)
                    {
                        ViewBag.Message = "Exist";
                        return View();
                    }
                    else
                    {
                        var food = restaurant.FoodId;
                        var prices = restaurant.Price;
                        if (food == null)
                        {
                            food = "";
                            restaurant.FoodId = Convert.ToString(getFood.Id);

                        }
                        else
                        {
                            restaurant.FoodId = food + "," + Convert.ToString(getFood.Id);
                        }
                        if (prices == null)
                        {
                            prices = "";
                            restaurant.Price = Convert.ToString(price);

                        }
                        else
                        {
                            restaurant.Price = prices + "," + Convert.ToString(price);

                        }
                        //restaurant.FoodId = food + "," + Convert.ToString(getFood.Id);
                        //restaurant.Price = prices + "," + Convert.ToString(price);
                        restaurantLogic.Update(restaurant);

                        ViewBag.Message = "Success";


                        return RedirectToAction("Index");

                    }
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