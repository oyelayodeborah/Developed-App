﻿using System;
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
        
        public Food SetRecommendedFoodOne(int id, DateTime lastLoggedIn)
        {
            var getcust = GetByUserID(id);
            var retrieveRecommendedFoodOne = new Food();
            IEnumerable<string> getFoodAllergies = null;

            if (getcust.RecommendedFoodOne!=null && lastLoggedIn.Date == DateTime.Now.Date)
            {
                retrieveRecommendedFoodOne = new FoodLogic().Get(Convert.ToInt32(getcust.RecommendedFoodOne));
            }
            else
            {
                var getAllFoodId = new FoodLogic().GetAllFoodId();
                var FoodAllergies = new List<int>();
                var getAll = _context.Foods.ToList();
                var varIndex = 0;
                var getAllergicFood = new Food();
                Random rand = new Random();
                varIndex = rand.Next(getAllFoodId.Count());
                retrieveRecommendedFoodOne = new FoodLogic().Get(getAllFoodId[varIndex]);
                getFoodAllergies = from allergies in getcust.FoodAllergies.Split(',')
                                   select Convert.ToString(allergies);
                var containsfood = getcust.FoodAllergies.Contains(retrieveRecommendedFoodOne.Name);
                do
                {
                    varIndex = rand.Next(getAllFoodId.Count());
                    retrieveRecommendedFoodOne = new FoodLogic().Get(getAllFoodId[varIndex]);
                    containsfood = getcust.FoodAllergies.Contains(retrieveRecommendedFoodOne.Name);
                } while (containsfood==true);
                getcust.RecommendedFoodOne = retrieveRecommendedFoodOne.Id.ToString();
                Repo.Update(getcust);

            }
            return retrieveRecommendedFoodOne;

        }

        public Food SetRecommendedFoodTwo(int id, DateTime lastLoggedIn)
        {
            var getcust = GetByUserID(id);
            var retrieveRecommendedFoodTwo = new Food();
            IEnumerable<string> getFoodAllergies = null;

            if (getcust.RecommendedFoodTwo != null && lastLoggedIn.Date == DateTime.Now.Date)
            {
                retrieveRecommendedFoodTwo = new FoodLogic().Get(Convert.ToInt32(getcust.RecommendedFoodTwo));
            }
            else
            {
                var getAllFoodId = new FoodLogic().GetAllFoodId();
                var retrieveRecommendedFoodOne = Convert.ToInt32(getcust.RecommendedFoodOne);
                if (getAllFoodId != null)
                {
                    getAllFoodId.Remove(retrieveRecommendedFoodOne);
                    var FoodAllergies = new List<int>();
                    var getAll = _context.Foods.ToList();
                    var varIndex = 0;
                    var getAllergicFood = new Food();
                    Random rand = new Random();
                    varIndex = rand.Next(getAllFoodId.Count());
                    retrieveRecommendedFoodTwo = new FoodLogic().Get(getAllFoodId[varIndex]);
                    getFoodAllergies = from allergies in getcust.FoodAllergies.Split(',')
                                       select Convert.ToString(allergies);
                    var containsfood = getcust.FoodAllergies.Contains(retrieveRecommendedFoodTwo.Name);

                    do
                    {
                        varIndex = rand.Next(getAllFoodId.Count());
                        retrieveRecommendedFoodTwo = new FoodLogic().Get(getAllFoodId[varIndex]);
                        containsfood = getcust.FoodAllergies.Contains(retrieveRecommendedFoodTwo.Name);
                    } while (containsfood == true || retrieveRecommendedFoodTwo.Id==retrieveRecommendedFoodOne);
                    getcust.RecommendedFoodTwo = retrieveRecommendedFoodTwo.Id.ToString();
                    Repo.Update(getcust);

                }
            }
            return retrieveRecommendedFoodTwo;

        }

        public Food SetRecommendedFoodThree(int id, DateTime lastLoggedIn)
        {
            var getcust = GetByUserID(id);
            var retrieveRecommendedFoodThree = new Food();
            IEnumerable<string> getFoodAllergies = null;

            if (getcust.RecommendedFoodThree != null && lastLoggedIn.Date == DateTime.Now.Date)
            {
                retrieveRecommendedFoodThree = new FoodLogic().Get(Convert.ToInt32(getcust.RecommendedFoodThree));
            }
            else
            {
                var getAllFoodId = new FoodLogic().GetAllFoodId();
                var retrieveRecommendedFoodTwo = Convert.ToInt32(getcust.RecommendedFoodTwo);
                var retrieveRecommendedFoodOne = Convert.ToInt32(getcust.RecommendedFoodOne);

                if (getAllFoodId != null)
                {
                    getAllFoodId.Remove(Convert.ToInt32(getcust.RecommendedFoodOne));
                    var FoodAllergies = new List<int>();
                    var getAll = _context.Foods.ToList();
                    var varIndex = 0;
                    var getAllergicFood = new Food();
                    Random rand = new Random();
                    varIndex = rand.Next(getAllFoodId.Count());
                    if (getAllFoodId.Count > 2)
                    {
                        retrieveRecommendedFoodThree = new FoodLogic().Get(getAllFoodId[varIndex]);
                        getFoodAllergies = from allergies in getcust.FoodAllergies.Split(',')
                                           select Convert.ToString(allergies);
                        var containsfood = getcust.FoodAllergies.Contains(retrieveRecommendedFoodThree.Name);
                        do
                        {
                            varIndex = rand.Next(getAllFoodId.Count());
                            retrieveRecommendedFoodThree = new FoodLogic().Get(getAllFoodId[varIndex]);
                            containsfood = getcust.FoodAllergies.Contains(retrieveRecommendedFoodThree.Name);
                        } while (containsfood == true || retrieveRecommendedFoodThree.Id == retrieveRecommendedFoodTwo || retrieveRecommendedFoodThree.Id == retrieveRecommendedFoodOne);
                    }
                    else { 
                        retrieveRecommendedFoodThree = new FoodLogic().Get(retrieveRecommendedFoodOne);
                    }
                    getcust.RecommendedFoodThree = retrieveRecommendedFoodThree.Id.ToString();
                    Repo.Update(getcust);

                }
            }
            return retrieveRecommendedFoodThree;

        }


        public IEnumerable<Customer> GetAll()
        {
            return Repo.GetAll();
        }


        public List<Food> SetRecommendedFood(int id, DateTime lastLoggedIn)
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
            List<int> gatherRemoveElements = new List<int>();

            if (item != null)
            {

                if (item.RecommendedFood != "")
                {

                    if (item.RecommendedFood.Contains(","))
                    {
                        getFoodId = from val in item.RecommendedFood.Split(',')
                                    select Convert.ToString(val);
                        foreach(var getid in getFoodId)
                        {
                            getFood = new FoodLogic().Get(Convert.ToInt32(getid));
                            if (getFood != null)
                            {
                                getAllFoodId.Add(new Food { Id = getFood.Id, Image = getFood.Image, ContentType = getFood.ContentType, Name = getFood.Name });
                            }
                        }
                    }
                    else
                    {
                        setFoodId.Add(item.RecommendedFood);
                        getFoodId = setFoodId;
                        foreach (var getid in getFoodId)
                        {
                            getFood = new FoodLogic().Get(Convert.ToInt32(getid));
                            if (getFood != null)
                            {
                                getAllFoodId.Add(new Food { Id = getFood.Id, Image = getFood.Image, ContentType = getFood.ContentType, Name = getFood.Name });
                            }
                        }
                    }
                }
                if (lastLoggedIn.Date != DateTime.Now.Date)
                {
                    item.RecommendedFood = "";
                    Repo.Update(item);
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
                                            gatherRemoveElements.Add(food.Id);

                                        }
                                        else
                                        {
                                            var foodid = food.Id;
                                            getAllFood.Add(foodid);
                                        }
                                    }
                                }
                                //else
                                //{
                                //    if (getAllFood.Count()==0)
                                //    {
                                //        int j = 0;
                                //        foreach (var allid in getAll)
                                //        {
                                //            getAllFood.Add(allid.Id);
                                //        }
                                //    }
                                //}
                            }
                            if (gatherRemoveElements.Count != 0)
                            {
                                foreach (var remove in gatherRemoveElements)
                                {
                                    var getRemove = getAll.SingleOrDefault(r => r.Id == remove);
                                    getAll.Remove(getRemove);
                                }

                            }

                            if (getAllFood.Count() == 0)
                            {
                                int j = 0;
                                foreach (var allid in getAll)
                                {
                                    getAllFood.Add(allid.Id);
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
                                        gatherRemoveElements.Add(food.Id);
                                    }
                                    else
                                    {
                                        getAllFood.Add(food.Id);
                                    }
                                }
                                if (gatherRemoveElements.Count != 0)
                                {
                                    foreach (var remove in gatherRemoveElements)
                                    {
                                        var getRemove = getAll.SingleOrDefault(r => r.Id == remove);
                                        getAll.Remove(getRemove);
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
                        var getIndex = getAllFood[varIndex];
                        bool findIndex = index.Contains(getIndex);
                        if (findIndex == false)
                        {
                            index[i] = Convert.ToInt32(getAllFood[varIndex]);
                        }
                        else
                        {
                            i--;
                        }
                    }
                    foreach (var value in index)
                    {
                        var itemvalue = Convert.ToInt32(value);
                        if (getAllFoodId.Count() < 3)
                        {
                            int IndexValue = index[key];
                            if (getAllFood.Count() != 0)
                            {
                                //foreach (var itemFood in getAllFood)
                                //{


                                //    if (getAllFoodId.Count() < 3)
                                //    {

                                getFood = foodLogic.Get(Convert.ToInt32(value));
                                var getRecommendFood = getAllFoodId.SingleOrDefault(r => r.Id == getFood.Id);
                                if (getRecommendFood == null)
                                {
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
                                    //    }
                                    //}

                                }
                            }
                        }
                        key++;

                    }
                    item.RecommendedFood = recommendedFood;
                    Repo.Update(item);


                //else
                //{
                //        getAllFoodId = new List<Food>();
                // }
                }
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