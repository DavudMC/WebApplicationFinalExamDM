using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace WebApplicationFinalExamDM.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
