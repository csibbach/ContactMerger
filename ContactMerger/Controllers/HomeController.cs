using System.Web.Mvc;

namespace ContactMerger.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["someVal"] = "Hello";
            return View();
        }
    }
}
