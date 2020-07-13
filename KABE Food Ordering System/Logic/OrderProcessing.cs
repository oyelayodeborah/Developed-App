using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KABE_Food_Ordering_System.Logic
{
    public class OrderProcessing : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var MySession = HttpContext.Current.Session;
            var sessionToString = Convert.ToString(MySession["OrderId"]);
            if (sessionToString==null || sessionToString=="")
            {
                filterContext.Result = new RedirectResult(string.Format("~/Home/Logout/"));
            }
        }
    }
}