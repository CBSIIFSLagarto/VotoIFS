using Core_RBS.Data;
using Core_RBS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nota2.ModelsView;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core_RBS.Controllers
{
    [Authorize(Policy = "Professor")]
    public class CampanhasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampanhasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Campanhas        
        public async Task<IActionResult> Index()
        {
            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            return View(await _context.Campanhas.Where(p => p.Usuario.UserName == usuario.UserName).ToListAsync());
        }

        // GET: Campanhas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            var campanha = await _context.Campanhas
                .FirstOrDefaultAsync(m => m.CamID == id && m.Usuario.Id == usuario.Id);
            if (campanha == null)
            {
                return NotFound();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                string url = HttpContext.Request.Host.Value + "/Votos/Votar/" + campanha.Chave;
                ViewBag.URL = url;
                QRCodeGenerator qr = new QRCodeGenerator();
                QRCodeData data = qr.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);
                using (Bitmap bitMap = code.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

            return View(campanha);
        }

        // GET: Campanhas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Campanhas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CamID,Chave,Descricao,DataHoraInicio,DataHoraFim,AutoAvaliacao")] Campanha campanha)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
                campanha.Usuario = usuario;
                _context.Add(campanha);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(campanha);
        }

        // GET: Campanhas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            var campanha = await _context.Campanhas.FirstOrDefaultAsync(p => p.CamID == id && p.Usuario.Id == usuario.Id);
            if (campanha == null)
            {
                return NotFound();
            }
            return View(campanha);
        }

        // POST: Campanhas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CamID,Chave,Descricao,DataHoraInicio,DataHoraFim,AutoAvaliacao")] Campanha campanha)
        {
            if (id != campanha.CamID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campanha);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampanhaExists(campanha.CamID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(campanha);
        }

        // GET: Campanhas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            var campanha = await _context.Campanhas.FirstOrDefaultAsync(m => m.CamID == id || m.Usuario.Id == usuario.Id);
            if (campanha == null)
            {
                return NotFound();
            }

            return View(campanha);
        }

        // POST: Campanhas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campanha = await _context.Campanhas.FindAsync(id);
            _context.Campanhas.Remove(campanha);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampanhaExists(int id)
        {
            return _context.Campanhas.Any(e => e.CamID == id);
        }


        public async Task<IActionResult> RelatorioVotos(int camId, int autoavaliacao, DateTime? minDate, DateTime? maxDate)
        {
            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            /*
            if (!minDate.HasValue)
            {
                minDate = DateTime.Now;
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd HH:mm");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd HH:mm");
            */
            var campanhas = FindAllUser(usuario.Id);
            var votos = await FindAllAsync(camId, autoavaliacao, minDate, maxDate, usuario.Id);
            var mediaVotos = GetMediaVotos(votos);
            var viewModel = new RelatorioViewModel { Campanhas = campanhas, Votos = votos, MediaVotos = mediaVotos};
            return View(viewModel);
        }
        public List<Campanha> FindAllUser(string userId)
        {
            return _context.Campanhas.Include(c => c.Usuario).Where(obj => obj.Usuario.Id == userId).OrderBy(x => x.Descricao).ToList();
        }
        public async Task<List<Voto>> FindAllAsync(int camId, int autoavaliacao, DateTime? minDate, DateTime? maxDate, string userId)
        {
            var result = from obj in _context.Votos select obj;
            if (camId > 0)
            {
                result = result.Where(x => x.CamId == camId);
            }
            if (autoavaliacao > 0)
            {
                result = result.Include(x => x.Campanha).Where(x => x.Campanha.AutoAvaliacao == true);
            }
            if (minDate.HasValue)
            {
                result = result.Where(x => x.DataVoto >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.DataVoto <= maxDate.Value);
            }

            return await result.Include(x => x.Campanha).Include(x => x.Campanha.Usuario).Where(x => x.Campanha.Usuario.Id == userId).OrderByDescending(x => x.DataVoto).ToListAsync();

        }
        public double GetMediaVotos(List<Voto> votos)
        {
            var soma = 0.0;
            var qtd = 0.0;
            foreach (var item in votos)
            {
                soma += item.Nota;
                qtd++;
            }
            if (qtd <= 0)
            {
                return 0;
            }
            return soma / qtd;
        }
    }
}
