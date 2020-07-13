using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KABE_Food_Ordering_System.Models;

namespace KABE_Food_Ordering_System.Logic
{
    public class RoleLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public BaseLogic<Role> roleRepo = new BaseLogic<Role>(new ApplicationDbContext());

        public IEnumerable<Role> GetAll()
        {
            return roleRepo.GetAll();
        }
        //Checks if the new role about to be created already exist in the database
        public bool IsDetailsExist(string value)
        {
            var findDetails = _context.Roles.Where(a => a.Name == value).Count();

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