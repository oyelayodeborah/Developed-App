using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KABE_Food_Ordering_System.Models;
using NinjaNye.SearchExtensions;

namespace KABE_Food_Ordering_System.Logic
{
    public class UserLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public BaseLogic<User> Repo = new BaseLogic<User>(new ApplicationDbContext());
        public User Get(int id)
        {
            var findDetails = _context.Users.Where(a => a.Id == id).SingleOrDefault();
            return findDetails;
        }
        public User FindByName(string value)
        {
            var findDetails = _context.Users.Where(a => a.Name == value).SingleOrDefault();
            return findDetails;
        }
        public User FindByStatus(string value)
        {
            var getStatus = Status.Active;
            if (value == "InActive")
            {
                getStatus = Status.InActive;
            }if (value == "ChangePassword")
            {
                getStatus = Status.ChangePassword;
            }if (value == "Deactivated")
            {
                getStatus = Status.Deactivated;
            }if (value == "ProfileNotCreated")
            {
                getStatus = Status.ProfileNotCreated;
            }
            var findDetails = _context.Users.Where(a => a.Status == getStatus).SingleOrDefault();
            return findDetails;
        }
        
        
        //Checks if the new role about to be created already exist in the database
        public bool IsDetailsExist(string value)
        {
            var findDetails = _context.Users.Where(a => a.Name == value).Count();

            if (findDetails == 0)
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