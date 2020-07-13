using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KABE_Food_Ordering_System.Models;
using KABE_Food_Ordering_System.ViewModels;

namespace KABE_Food_Ordering_System.Logic
{
    public class CustomerLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public BaseLogic<Customer> Repo = new BaseLogic<Customer>(new ApplicationDbContext());
        public BaseLogic<Food> foodLogic = new BaseLogic<Food>(new ApplicationDbContext());

        public IEnumerable<Customer> GetAll()
        {
            return Repo.GetAll();
        }

        public List<Food> SetRecommendedFood(int id)
        {
            var item = GetByUserID(id);
            var getAllFoodId = new List<Food>();
            var FoodAllergies = new List<int>();
            var getAll = _context.Foods.ToList();
            //int[] getAllFood = { };
            List<int> getAllFood = new List<int>();
            Food getFood = new Food();
            IEnumerable<string> getFoodId = null;
            List<string> setFoodId = new List<string>();
            IEnumerable<string> getFoodAllergies = null;
            string[] newArray = { };

            if (item != null)
            {

                if (item.RecommendedFood != null)
                {

                    if (item.RecommendedFood.Contains(","))
                    {
                        getFoodId = from val in item.RecommendedFood.Split(',')
                                    select Convert.ToString(val);
                    }
                    else
                    {
                        setFoodId.Add(item.RecommendedFood);
                        getFoodId = setFoodId;
                    }
                }
                string foodallergies = item.FoodAllergies;
                if (foodallergies != null || foodallergies != "")
                {
                    if (item.FoodAllergies.Contains(","))
                    {
                        getFoodAllergies = from allergies in item.FoodAllergies.Split(',')
                                           select Convert.ToString(allergies);
                        foreach (var foodall in getFoodAllergies)
                        {
                            getFood = new FoodLogic().FindFoodByName(foodall);
                            if (getFood != null)
                            {
                                foreach (var food in getAll)
                                {
                                    if (food.Id == getFood.Id)
                                    {
                                        getAll.Remove(new Food { Id = food.Id, Image = food.Image, ContentType = food.ContentType, Name = food.ContentType });
                                    }
                                    else
                                    {
                                        getAllFood.Add(food.Id);
                                    }
                                }
                            }
                            else
                            {
                                if (getAllFood.Count()==0)
                                {
                                    int j = 0;
                                    foreach (var allid in getAll)
                                    {
                                        getAllFood.Add(allid.Id);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        getFood = new FoodLogic().FindFoodByName(item.FoodAllergies);
                        if (getFood != null)
                        {
                            foreach (var food in getAll)
                            {
                                if (food.Id == getFood.Id)
                                {
                                    getAll.Remove(new Food { Id = food.Id, Image = food.Image, ContentType = food.ContentType, Name = food.ContentType });
                                }
                                else
                                {
                                    getAllFood.Add(food.Id);
                                }
                            }
                        }
                        else
                        {
                            int j = 0;
                            foreach (var allid in getAll)
                            {
                                getAllFood.Add(allid.Id);
                            }
                        }
                    }
                }
                else
                {
                    int j = 0;
                    foreach (var allid in getAll)
                    {
                        getAllFood.Add(allid.Id);
                    }
                }


                int[] index = new int[3];
                string recommendedFood = "";
                int key = 0;
                var varIndex = 0;

                var countIndex = getAll.Count;
                if (countIndex > 3)
                {
                    countIndex = 3;
                }
                for (int i = 0; i < countIndex; i++)
                {
                    Random rand = new Random();
                    // Generate a random index less than the size of the array.
                    varIndex = rand.Next(getAllFood.Count());

                    index[i] = Convert.ToInt32(getAllFood[varIndex]);
                }
                foreach (var value in index)
                {
                    var itemvalue = Convert.ToInt32(value);
                    if (key < 3 && key < getAllFood.Count())
                    {
                        int IndexValue = index[key];
                        if (getAllFood.Count() != 0)
                        {
                            foreach (var itemFood in getAllFood)
                            {

                                if (itemFood != itemvalue)
                                {
                                    if (getAllFoodId.Count() <= 3)
                                    {

                                        getFood = foodLogic.Get(Convert.ToInt32(getAllFood[key]));
                                        if (recommendedFood == "")
                                        {
                                            recommendedFood = getFood.Id.ToString();
                                        }
                                        else
                                        {
                                            var addMore = recommendedFood + "," + getFood.Id.ToString();
                                            recommendedFood = addMore;
                                        }
                                        getAllFoodId.Add(new Food { Id = getFood.Id, Image = getFood.Image, ContentType = getFood.ContentType, Name = getFood.Name });
                                    }

                                }
                            }
                        }
                    }
                    key++;

                }
                item.RecommendedFood = recommendedFood;
                Repo.Update(item);

            }
            else
            {
                getAllFoodId = new List<Food>();
            }
            return getAllFoodId;
        }

        public Customer GetByUserID(int id)
        {
            var findDetails = _context.Customers.Where(a => a.UsersId == id).SingleOrDefault();
            return findDetails;
        }
        public Customer Get(int id)
        {
            var findDetails = _context.Customers.Where(a => a.Id == id).SingleOrDefault();
            return findDetails;
        }
        //Checks if the new role about to be created already exist in the database
        public bool IsDetailsExist(string value)
        {
            var findDetails = _context.Customers.Where(a => a.Name == value).Count();

            if (findDetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Customer GetCustomerByEmail(string Email)
        {
            var finddetails = _context.Customers.Where(a => a.Email == Email).FirstOrDefault();

            return finddetails;
        }
    }
}