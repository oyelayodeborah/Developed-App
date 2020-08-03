using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Models;

namespace KABE_Food_Ordering_System.Logic
{
    public class FoodLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public BaseLogic<Food> foodRepo = new BaseLogic<Food>(new ApplicationDbContext());
        public Food Get(int? id)
        {
            return foodRepo.Get(id);
        }
        public void Save(Food food)
        {
            foodRepo.Save(food);
        }
        public Food FindByName(string value)
        {
            var findDetails = _context.Foods.Where(a => a.Name == value).SingleOrDefault();
            return findDetails;
        }
        public Food FindFoodByName(string value)
        {
            var findDetails = _context.Foods.Where(a => a.Name.ToLower().Trim() == value.ToLower().Trim()).SingleOrDefault();
            return findDetails;
        }
        public List<int> GetAllFoodId()
        {
            var getFood = _context.Foods.ToList();
            var getAll = new List<int>();
            if (getFood.Count() != 0)
            {
                foreach (var item in getFood)
                {
                    getAll.Add(item.Id);
                }
            }
            else
            {
                getAll = new List<int>();
            }
            return getAll;
        }

        //Checks if the new food about to be created already exist in the database
        public bool IsDetailsExist(string value)
        {
            var findDetails = _context.Foods.Where(a => a.Name == value).Count();

            if (findDetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Checks if the new food about to be created already exist in the database
        public bool IsRestaurantExist(Dictionary<int, string> item, int Id)
        {
            var findDetails = item.ContainsKey(Id);

            if (findDetails == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
    }
}