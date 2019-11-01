using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nota2.Data;
using Nota2.Models;
using Microsoft.AspNetCore.Http;

namespace Nota2.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;
        public const string SessionKeyId = "_Id";

        public HomeController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionKeyId) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }            
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
