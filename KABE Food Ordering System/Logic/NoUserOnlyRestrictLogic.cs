using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KABE_Food_Ordering_System.Models;
namespace KABE_Food_Ordering_System.Logic
{
    public class NoUserOnlyRestrictLogic : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var MySession = HttpContext.Current.Session;
            var _context = new BaseLogic <User>( new ApplicationDbContext());
            var find = _context.GetAll().Count();
            var sessionToString = (string)MySession["role"];
            if (sessionToString != null && find !=0)
            {
                filterContext.Result = new RedirectResult(string.Format("~/Home/Logout/"));
            }
        }
    }
}
    
