using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Models;

namespace KABE_Food_Ordering_System.Logic
{
    public class LocationLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public BaseLogic<Location> locationRepo = new BaseLogic<Location>(new ApplicationDbContext());
        public Location Get(int? id)
        {
            return locationRepo.Get(id);
        }
        public IEnumerable<Location> GetAll()
        {
            return locationRepo.GetAll();
        }
        //Checks if the new role about to be created already exist in the database
        public bool IsDetailsExist(string value)
        {
            var findDetails = _context.Locations.Where(a => a.Name == value).Count();

            if (findDetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Location FindByName(string value)
        {
            var findDetails = _context.Locations.Where(a => a.Name == value).SingleOrDefault();
            return findDetails;
        }
        
        public Location FindByState(string value)
        {
            Location location = new Location();
            var state= State.Lagos;
            if (location.State.ToString() == value)
            {
                state = location.State;
            }
            var findDetails = _context.Locations.Where(a => a.State == state).SingleOrDefault();
            return findDetails;
        }
    }
}