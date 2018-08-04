using System.Web.Mvc;

namespace RedisCache.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            object value = Session["Home"];
            if (value != null)
            {
                var result = (int)value;
                Session["Home"] = ++result;
            }
            else
            {
                Session["Home"] = 1;
            }
            ViewBag.NoOfView = Session["Home"];
            return View();
        }
    }
}