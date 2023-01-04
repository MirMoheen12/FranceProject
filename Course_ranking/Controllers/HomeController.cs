using Course_ranking.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Course_ranking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CourseContexte courseContexte;

        public HomeController(ILogger<HomeController> logger, CourseContexte courseContexte)
        {
            _logger = logger;
            this.courseContexte = courseContexte;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string useremail,string Password)
        {
            var data = courseContexte.AllUsers.Where(x => x.useremail == useremail && x.userePassword == Password).FirstOrDefault();
            if (data != null)
            {
                HttpContext.Session.SetString("UserEmail", data.useremail);
                return RedirectToAction("Index","Dashboard");
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(AllUsers allUsers)
        {
            courseContexte.AllUsers.Add(allUsers);
            courseContexte.SaveChanges();
            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}