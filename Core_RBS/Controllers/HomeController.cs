using Core_RBS.Data;
using Core_RBS.Models;
using Core_RBS.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace Core_RBS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
                var campanhas = _context.Campanhas.Include(c => c.Votos).Where(p => p.Usuario.UserName == usuario.UserName && p.DataHoraInicio < DateTime.Now && p.DataHoraFim > DateTime.Now).OrderBy(c => c.DataHoraFim).ToList();
                var viewModel = new HomeViewModel { Campanhas = campanhas };
                return View(viewModel);
            }
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
