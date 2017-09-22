using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace ContactMerger.Controllers
{
    [Authorize]
    [ExcludeFromCodeCoverage]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
