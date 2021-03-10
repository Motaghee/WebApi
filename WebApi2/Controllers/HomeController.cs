using System.Web.Mvc;

namespace WebApi2.Controllers
{
    public class HomeController : Controller
    {
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
