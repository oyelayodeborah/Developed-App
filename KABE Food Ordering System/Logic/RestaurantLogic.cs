using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KABE_Food_Ordering_System.Models;
using KABE_Food_Ordering_System.ViewModels;
using NinjaNye.SearchExtensions;

namespace KABE_Food_Ordering_System.Logic
{
    public class RestaurantLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public BaseLogic<Restaurant> Repo = new BaseLogic<Restaurant>(new ApplicationDbContext());
        public BaseLogic<Food> foodLogic = new BaseLogic<Food>(new ApplicationDbContext());

        public IEnumerable<Restaurant> GetAll(){
            return Repo.GetAll();
        }
        public Restaurant FindByName(string value)
        {
            var findDetails = _context.Restaurants.Where(a => a.Name == value).SingleOrDefault();
            return findDetails;
        }
        public List<decimal> GetListRestaurantFoodsPrice()
        {
            var restaurant = _context.Restaurants.ToList();
            List<decimal> getPrice = new List<decimal>();
            var getAllPrice = new List<decimal>();
            FoodViewModels getFood = new FoodViewModels();
            if (restaurant != null)
            {
                foreach (var item in restaurant)
                {
                    var getLocation = new LocationLogic().Get(item.LocationId);
                    var findFood = new FoodLogic().Get(Convert.ToInt32(item.FoodId));


                    if (item.FoodId.Contains(",") && item.Price.Contains(","))
                    {
                        string[] splitPrice = item.Price.Split(',');

                        foreach (var val in splitPrice)
                        {
                            getPrice.Add(Convert.ToDecimal(val));

                        }

                    }
                    else
                    {
                        if (item.Price != "" || item.Price != null)
                        {
                            var price = Convert.ToDecimal(item.Price);
                            getPrice.Add(price);

                        }
                    }
                }
            }
            else
            {
                getPrice = new List<decimal>();
            }
            return getPrice;
        }
        public List<decimal> GetListRestaurantFoodsPrice(string searchTerm)
        {
            var restaurant = _context.Restaurants.ToList();
            List<decimal> getPrice = null;
            var getAllPrice = new List<decimal>();
            FoodViewModels getFood = new FoodViewModels();
            if (restaurant != null)
            {
                foreach (var item in restaurant)
                {
                    var getLocation = new LocationLogic().Get(item.LocationId);
                    var findFood = new FoodLogic().Get(Convert.ToInt32(item.FoodId));
                    if (getLocation != null)
                    {
                        if (getLocation.Name.Contains(searchTerm))
                        {
                            if (item.FoodId.Contains(",") && item.Price.Contains(","))
                            {
                                string[] splitPrice = item.Price.Split(',');

                                foreach (var val in splitPrice)
                                {
                                    getPrice.Add(Convert.ToDecimal(val));

                                }

                            }
                            else
                            {
                                getPrice.Add(Convert.ToDecimal(item.Price));
                            }
                        }
                        else if (item.Name.Contains(searchTerm))
                        {
                            if (item.FoodId.Contains(",") && item.Price.Contains(","))
                            {
                                string[] splitPrice = item.Price.Split(',');

                                foreach (var val in splitPrice)
                                {
                                    getPrice.Add(Convert.ToDecimal(val));

                                }

                            }
                            else
                            {
                                getPrice.Add(Convert.ToDecimal(item.Price));
                            }
                        }
                        else if (findFood.Name.Contains(searchTerm))
                        {
                            if (item.FoodId.Contains(",") && item.Price.Contains(","))
                            {
                                string[] splitPrice = item.Price.Split(',');

                                foreach (var val in splitPrice)
                                {
                                    getPrice.Add(Convert.ToDecimal(val));

                                }

                            }
                            else
                            {
                                getPrice.Add(Convert.ToDecimal(item.Price));
                            }
                        }
                    }


                }
            }
            else
            {
                getPrice = new List<decimal>();
            }
            return getPrice;
        }
        public List<FoodViewModels> GetListRestaurantFoods(string searchTerm)
        {
            var restaurant = _context.Restaurants.ToList();
            var getAllFoodId = new List<FoodViewModels>();
            var findFood = new Food();
            var getVal = new List<string>();
            IEnumerable<string> getFoodId = null;
            string[] newArray = { };

            FoodViewModels getFood = new FoodViewModels();


            if (restaurant != null)
            {
                foreach (var item in restaurant)
                {
                    if (item.FoodId.Contains(","))
                    {
                        getFoodId = from val in item.FoodId.Split(',')
                                    select Convert.ToString(val);


                        if (getFoodId != null)
                        {
                            newArray = getFoodId.ToArray();
                            var count = newArray.Length - 1;
                            for (int i = 0; i < count; i++)
                            {
                                if (newArray[i] != "0")
                                {
                                    var key = Convert.ToInt32(newArray[i]);
                                    var Price = Convert.ToDecimal(newArray[i + 1]);

                                    findFood = foodLogic.Get(key);
                                    if (findFood != null)
                                    {
                                        var findLocation = new LocationLogic().Get(item.LocationId);

                                        getAllFoodId.Add(new FoodViewModels
                                        {
                                            Id = findFood.Id,
                                            RestaurantName = item.Name,
                                            ContentType = findFood.ContentType,
                                            Name = findFood.Name,
                                            Location = findLocation.Name,
                                            Image = findFood.Image,
                                            Price = Price,
                                            About = item.About
                                        });
                                        i++;
                                    }
                                }

                            }
                            }
                        }
                    }
                }
            else
            {
                getAllFoodId = new List<FoodViewModels>();
            }
            return getAllFoodId;
        }
        public List<FoodViewModels> GetFoodsByRestaurant(string searchTerm)
        {
            var restaurant = _context.Restaurants.ToList();
            var getAllFoodId = new List<FoodViewModels>();
            var findFood = new Food();
            var getVal = new List<string>();
            var getRestaurant = new List<Restaurant>();
            IEnumerable<string> getFoodId = null;
            FoodViewModels getFood = new FoodViewModels();

            string[] newArray = { };

            if (restaurant != null)
            {

                foreach (var item in restaurant)
                {
                    getVal.Add(item.Name);
                    if (item.Name.Contains(searchTerm))
                    {
                        getRestaurant.Add(item);
                    }
                }
                if (getRestaurant != null)
                {
                    foreach (var value in getRestaurant)
                    {
                        if (value.FoodId != null)
                        {

                            if (value.FoodId.Contains(","))
                            {
                                getFoodId = from val in value.FoodId.Split(',')
                                            select Convert.ToString(val);
                                if (getFoodId != null)
                                {
                                    newArray = getFoodId.ToArray();
                                    var count = newArray.Length - 1;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (newArray[i] != "0")
                                        {
                                            var key = Convert.ToInt32(newArray[i]);
                                            var Price = Convert.ToDecimal(newArray[i + 1]);

                                            findFood = foodLogic.Get(key);
                                            if (findFood != null)
                                            {
                                                var findLocation = new LocationLogic().Get(value.LocationId);

                                                getAllFoodId.Add(new FoodViewModels
                                                {
                                                    Id = findFood.Id,
                                                    RestaurantName = value.Name,
                                                    ContentType = findFood.ContentType,
                                                    Name = findFood.Name,
                                                    Location = findLocation.Name,
                                                    Image = findFood.Image,
                                                    Price = Price,
                                                    About = value.About
                                                });
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                getAllFoodId = new List<FoodViewModels>();
            }
            return getAllFoodId;
        }
        public List<FoodViewModels> GetFoodsByLocation(string searchTerm)
        {
            var restaurant = _context.Restaurants.ToList();
            var getAllFoodId = new List<FoodViewModels>();
            var findFood = new Food();
            var getVal = new List<string>();
            var getLocation = new List<Restaurant>();
            FoodViewModels getFood = new FoodViewModels();
            IEnumerable<string> getFoodId = null;
            string[] newArray = { };

            if (restaurant != null)
            {

                foreach (var item in restaurant)
                {
                    var location = new LocationLogic().Get(item.LocationId);
                    if (location != null)
                    {
                        getVal.Add(location.Name);
                        if (location.Name.Contains(searchTerm))
                        {
                            getLocation.Add(item);
                        }
                    }

                }
                if (getLocation != null)
                {
                    foreach (var value in getLocation)
                    {
                        if (value.FoodId != null)
                        {

                            if (value.FoodId.Contains(","))
                            {
                                getFoodId = from val in value.FoodId.Split(',')
                                            select Convert.ToString(val);
                                if (getFoodId != null)
                                {
                                    newArray = getFoodId.ToArray();
                                    var count = newArray.Length - 1;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (newArray[i] != "0")
                                        {
                                            var key = Convert.ToInt32(newArray[i]);
                                            var Price = Convert.ToDecimal(newArray[i + 1]);

                                            findFood = foodLogic.Get(key);
                                            if (findFood != null)
                                            {
                                                var findLocation = new LocationLogic().Get(value.LocationId);

                                                getAllFoodId.Add(new FoodViewModels
                                                {
                                                    Id = findFood.Id,
                                                    RestaurantName = value.Name,
                                                    ContentType = findFood.ContentType,
                                                    Name = findFood.Name,
                                                    Location = findLocation.Name,
                                                    Image = findFood.Image,
                                                    Price = Price,
                                                    About = value.About
                                                });
                                                i++;
                                            }
                                        }
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
            else
            {
                getAllFoodId = new List<FoodViewModels>();
            }
            return getAllFoodId;
        }
        public List<int> GetAllRestaurantFoods(int id)
        {
            var restaurant = _context.Restaurants.Where(a => a.RestaurantsId == id).SingleOrDefault();
            var getAllFoodId = new List<int>();

            if (restaurant != null)
            {
                var getFoodId = from val in restaurant.FoodId.Split(',')
                                select int.Parse(val);
                foreach (int value in getFoodId)
                {
                    if (value != 0)
                    {
                        getAllFoodId.Add(value);
                    }
                }
            }
            else
            {
                getAllFoodId = new List<int>();
            }
            return getAllFoodId;
        }
        public List<FoodViewModels> GetAllRestaurantFoodsList(int id)
        {
            var item = GetByUserID(id);
            var getAllFoodId = new List<FoodViewModels>();
            FoodViewModels getFood = new FoodViewModels();
            IEnumerable<string> getFoodId = null;
            string[] newArray = { };

            if (item != null)
            {
                
                    if (item.FoodId != null)
                    {

                        if (item.FoodId.Contains(","))
                        {
                            getFoodId = from val in item.FoodId.Split(',')
                                        select Convert.ToString(val);
                            if (getFoodId != null)
                            {
                                newArray = getFoodId.ToArray();
                                var count = newArray.Length - 1;
                            for (int i = 0; i < count; i++)
                            {
                                if (newArray[i] != "0")
                                {
                                    var key = Convert.ToInt32(newArray[i]);
                                    var Price = Convert.ToDecimal(newArray[i + 1]);

                                    var findFood = foodLogic.Get(key);
                                    if (findFood != null)
                                    {
                                        var findLocation = new LocationLogic().Get(item.LocationId);

                                        getAllFoodId.Add(new FoodViewModels
                                        {
                                            Id = findFood.Id,
                                            RestaurantName = item.Name,
                                            ContentType = findFood.ContentType,
                                            Name = findFood.Name,
                                            Location = findLocation.Name,
                                            Image = findFood.Image,
                                            Price = Price,
                                            About = item.About
                                        });
                                        i++;
                                    }
                                }
                            }
                            }
                            }
                        }
                    }
            else
            {
                getAllFoodId = new List<FoodViewModels>();
            }
            return getAllFoodId;
        }

        public List<FoodViewModels> GetAllRestaurantFoodsList()
        {
            var restaurant = _context.Restaurants.ToList();
            var getAllFood = new List<FoodViewModels>();
            FoodViewModels[] getAll = { };
            var NogetAllFoodId = new List<FoodViewModels>();
            FoodViewModels getFood = new FoodViewModels();
            IEnumerable<string> getFoodId = null;
            string[] newArray = { };

            if (restaurant != null)
            {
                foreach (var item in restaurant)
                {
                    if (item.FoodId != null)
                    {

                        if (item.FoodId.Contains(","))
                        {
                            getFoodId = from val in item.FoodId.Split(',')
                                        select Convert.ToString(val);
                            if (getFoodId != null)
                            {
                                newArray = getFoodId.ToArray();
                                var count = newArray.Length - 1;
                                for (int i = 0; i < count; i++)
                                {
                                    if (newArray[i] != "0")
                                    {
                                        var key = Convert.ToInt32(newArray[i]);
                                        var Price = Convert.ToDecimal(newArray[i +1]);

                                        var findFood = foodLogic.Get(key);
                                        Price = Convert.ToDecimal(newArray[i + 1]);
                                        if (findFood != null)
                                        {
                                            var findLocation = new LocationLogic().Get(item.LocationId);
                                            
                                            getAllFood.Add(new FoodViewModels { Id = findFood.Id, RestaurantName = item.Name, ContentType = findFood.ContentType, Name= findFood.Name,
                                                Location =findLocation.Name,Image= findFood.Image,Price=Price,About=item.About});
                                        }
                                    }
                                    
                                    i++;
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                getAllFood = new List<FoodViewModels>();
            }
            return getAllFood;
            
        }

        public List<string> GetAllRestaurantFoodPrices(int id)
        {
            var restaurant = _context.Restaurants.Where(a => a.RestaurantsId == id).SingleOrDefault();
            var getAllPrice = new List<string>();

            if (restaurant != null)
            {
                var getPrice = from val in restaurant.Price.Split(',')
                               select Convert.ToString(val);
                foreach (string value in getPrice)
                {
                    if (value != "0")
                    {
                        getAllPrice.Add(value);
                    }
                }
            }
            else
            {
                getAllPrice = new List<string>();
            }
            return getAllPrice;
        }
        public Restaurant GetByUserID(int id)
        {
            var findDetails = _context.Restaurants.Where(a => a.RestaurantsId == id).SingleOrDefault();
            return findDetails;
        }
        public Restaurant Get(int id)
        {
            var findDetails = _context.Restaurants.Where(a => a.Id == id).SingleOrDefault();
            return findDetails;
        }
        public List<Restaurant> FindByContainName(string value)
        {
            List<Restaurant> AddItems = null;
            var result = _context.Restaurants.Search(x => x.Name, x => x.ResidentialAddress).Containing(value).ToList();
            if (result == null)
            {
                var location = _context.Locations.Search(x => x.Name, x => x.State.ToString()).Containing(value).First();

                var getRestaurantLocation = _context.Restaurants.Where(x => x.LocationId == location.Id).ToList();
                if (getRestaurantLocation != null)
                {
                    foreach (var locs in getRestaurantLocation)
                    {
                        AddItems.Add(locs);
                    }

                }

            }
            else
            {
                foreach (var val in result)
                {
                    AddItems.Add(val);
                }
            }


            return AddItems;
        }
        public Restaurant FindByLocation(int value)
        {
            var findDetails = _context.Restaurants.Where(a => a.LocationId == value).SingleOrDefault();
            return findDetails;
        }
        public List<Restaurant> FindByLocationId(int value)
        {
            var findDetails = _context.Restaurants.Where(a => a.LocationId == value).ToList();
            return findDetails;
        }

        //Checks if the new role about to be created already exist in the database
        public bool IsDetailsExist(string value)
        {
            var findDetails = _context.Restaurants.Where(a => a.Name == value).Count();

            if (findDetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public Restaurant GetRestaurantByEmail(string Email)
        {
            var finddetails = _context.Restaurants.Where(a => a.Email == Email).FirstOrDefault();

            return finddetails;
        }
    }
}