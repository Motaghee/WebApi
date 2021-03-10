using System.Web.Mvc;

namespace GWSQC.saipacorp.com.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
