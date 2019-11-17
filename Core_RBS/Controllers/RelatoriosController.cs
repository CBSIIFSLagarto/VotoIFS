using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_RBS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core_RBS.ModelsViews;
using Core_RBS.Models;
using Core_RBS.ModelsView;
using Microsoft.EntityFrameworkCore;

namespace Core_RBS.Controllers
{
    [Authorize(Policy = "Professor")]
    public class RelatoriosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("DataInicio,DataFim")] Relatorio relatorio)
        {
            if (ModelState.IsValid)
            {
                relatorio.Campanha = new List<Campanha>();
                var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
                var camapanhas = _context.Campanhas.Where(p => p.Usuario.Id == usuario.Id && p.DataHoraFim.Date >= relatorio.DataInicio.Date && p.DataHoraInicio.Date <= relatorio.DataFim.Date && p.AutoAvaliacao).ToList();
                int notas = 0;
                int cont = 0;
                foreach (var item in camapanhas)
                {
                    item.Votos = _context.Votos.Where(p => p.CamId == item.CamID).ToList();
                    notas += item.Votos.Sum(p => p.Nota);
                    cont += item.Votos.Count();
                    relatorio.Campanha.Add(item);
                }
                if (notas == 0)
                {
                    ViewBag.MediaGeral = 0;
                }
                else
                {
                    ViewBag.MediaGeral = Math.Round(notas / (double)cont, 2);
                }                
                return View("Relatorio", relatorio);
            }
            return View();
        }
        public IActionResult RelatorioVotos()
        {
            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            var viewModel = new RelatorioViewModel { ListCampanhas = _context.Campanhas.Where(c => c.Usuario.Id == usuario.Id).ToList() };
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> RelatorioVotos(int camId, int autoavaliacao, DateTime? minDate, DateTime? maxDate)
        {

            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            var campanhas = FindAllAsync(camId, autoavaliacao, minDate, maxDate);                        
            var viewModel = new RelatorioViewModel { Campanhas = campanhas, ListCampanhas = _context.Campanhas.Where(c => c.Usuario.Id == usuario.Id).ToList()};
            return View(viewModel);
        }

        public  List<Campanha> FindAllAsync(int camId, int autoavaliacao, DateTime? minDate, DateTime? maxDate)
        {
            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            var result = _context.Campanhas.Where(obj => obj.Usuario.Id == usuario.Id);
            if (camId > 0)
            {
                result = result.Where(p => p.CamID == camId);
            }
            if (autoavaliacao == 0)
            {
                result = result.Where(p => !p.AutoAvaliacao);
            }
            if (minDate.HasValue)
            {
                result = result.Where(x => x.DataHoraInicio >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.DataHoraFim <= maxDate.Value);
            }
            List<Campanha> campanhas = result.ToList();
            List <Campanha> listTemp = new List<Campanha>();           
            int notas = 0;
            int cont = 0;
            foreach (var campanha in campanhas)
            {
                campanha.Votos = _context.Votos.Where(p => p.CamId == campanha.CamID).ToList();
                notas += campanha.Votos.Sum(p => p.Nota);
                cont += campanha.Votos.Count();
                listTemp.Add(campanha);
            }
            if (notas == 0)
            {
                ViewBag.MediaGeral = 0;
            }
            else
            {
                ViewBag.MediaGeral = Math.Round(notas / (double)cont, 2);
                ViewBag.TotalVotos = cont;
            }
            return listTemp;
        }
    }
}