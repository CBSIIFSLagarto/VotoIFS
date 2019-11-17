using Core_RBS.Data;
using Core_RBS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core_RBS.Util;

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

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            var campanhas = from s in _context.Campanhas.Where(p => p.Usuario.UserName == usuario.UserName) select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                campanhas = campanhas.Where(s => s.Descricao.Contains(searchString) || s.Chave.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    campanhas = campanhas.OrderByDescending(s => s.Descricao);
                    break;
                case "Date":
                    campanhas = campanhas.OrderBy(s => s.DataHoraInicio);
                    break;
                case "date_desc":
                    campanhas = campanhas.OrderByDescending(s => s.DataHoraInicio);
                    break;
                default:
                    campanhas = campanhas.OrderByDescending(s => s.DataHoraFim);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Campanha>.CreateAsync(campanhas.AsNoTracking(), pageNumber ?? 1, pageSize));            
        }

        // GET: Campanhas        
        //public async Task<IActionResult> Index(int id)
        //{            
        //    var usuario = _context.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
        //    var campanhas = await _context.Campanhas.Where(p => p.Usuario.UserName == usuario.UserName).OrderByDescending(p => p.DataHoraFim).ToListAsync();
        //    return View(campanhas);
        //}

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
    }
}
