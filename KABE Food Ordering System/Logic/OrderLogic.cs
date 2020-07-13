using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Models;

namespace KABE_Food_Ordering_System.Logic
{
    public class OrderLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public BaseLogic<Order> orderRepo = new BaseLogic<Order>(new ApplicationDbContext());
        public Order Get(int? id)
        {
            return orderRepo.Get(id);
        }
        public List<Order> GetAll()
        {
            return orderRepo.GetAll().ToList();
        }
        public List<Order> GetCustomerOrders(int id)
        {
            var getDetails = _context.Order.Where(c => c.CustomerId == id).ToList();
            if (getDetails == null)
            {
                getDetails = new List<Order>();
            }
            return getDetails;
        }
        public List<Order> GetRestaurantOrders(int id)
        {
            var getDetails = _context.Order.Where(c => c.RestaurantId == id && c.Status!=OrderStatus.NotPaid && c.Status!=OrderStatus.Cancelled).ToList();
            if (getDetails == null)
            {
                getDetails = new List<Order>();
            }
            return getDetails;
        }
        public void Save(Order order)
        {
            orderRepo.Save(order);
        }
        //public Food FindByName(string value)
        //{
        //    var findDetails = _context.Order.Where(a => a.Name == value).SingleOrDefault();
        //    return findDetails;
        //}

        //public Food FindByRestaurant(int value)
        //{
        //    var findDetails = _context.Orders.Where(a => a.Restaurants.ContainsKey(value)).SingleOrDefault();
        //    return findDetails;
        //}

        //Checks if the new order about to be created already exist in the database
        //public bool IsDetailsExist(string value)
        //{
        //    var findDetails = _context.Orders.Where(a => a.Name == value).Count();

        //    if (findDetails == 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ////Checks if the new order about to be created already exist in the database
        //public bool IsRestaurantExist(Dictionary<int, string> item, int Id)
        //{
        //    var findDetails = item.ContainsKey(Id);

        //    if (findDetails == true)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public IEnumerable<T> SearchOrder(string order, string restaurant, string location)
        //{

        //}
    }
}