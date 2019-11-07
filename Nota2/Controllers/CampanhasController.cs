using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nota2.Data;
using Nota2.Models;
using Nota2.Services;
using QRCoder;

namespace Nota2.Controllers
{
    public class CampanhasController : Controller
    {
        private readonly MyContext _context;
        public const string SessionKeyId = "_Id";
        private readonly VotosService _votosService;

        public CampanhasController(MyContext context, VotosService votosService)
        {
            _context = context;
            _votosService = votosService;
        }

        // GET: Campanhas
        public async Task<IActionResult> Index()
        {
            var sessao = HttpContext.Session.GetString(SessionKeyId);
            if (sessao != null)
            {
                var myContext = _context.Campanhas.Where(p => p.UseId.Equals(Convert.ToInt32(sessao))).Include(c => c.Usuario);
                return View(await myContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }            
        }

        // GET: Campanhas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var sessao =Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            if (id == null)
            {
                return NotFound();
            }

            var campanha = await _context.Campanhas
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.CamID == id && m.UseId == sessao);
            if (campanha == null)
            {
                //return NotFound();
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
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
            var sessao = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            if (sessao.Equals(null))
            {
                return NotFound();
            }
            ViewData["UseId"] = new SelectList(_context.Usuarios, "UseID", "Email");
            return View();
        }

        // POST: Campanhas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CamID,Chave,Descricao,DataHoraInicio,DataHoraFim,AutoAvaliacao,UseId")] Campanha campanha)
        {
            var sessao = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            if (sessao.Equals(null))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(campanha);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UseId"] = new SelectList(_context.Usuarios, "UseID", "Email", campanha.UseId);
            return View(campanha);
        }

        // GET: Campanhas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var sessao = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            if (id == null)
            {
                return NotFound();
            }

            //var campanha = await _context.Campanhas.FindAsync(id);
            var campanha = await _context.Campanhas.FirstOrDefaultAsync(p => p.CamID == id && p.UseId == sessao);
            if (campanha == null)
            {
                //return NotFound();
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            ViewData["UseId"] = new SelectList(_context.Usuarios, "UseID", "Email", campanha.UseId);
            return View(campanha);
        }

        // POST: Campanhas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CamID,Chave,Descricao,DataHoraInicio,DataHoraFim,AutoAvaliacao,UseId")] Campanha campanha)
        {
            var sessao = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            if (sessao.Equals(null))
            {
                return NotFound();
            }

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
            ViewData["UseId"] = new SelectList(_context.Usuarios, "UseID", "Email", campanha.UseId);
            return View(campanha);
        }

        // GET: Campanhas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var sessao = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            if (id == null)
            {
                return NotFound();
            }

            var campanha = await _context.Campanhas
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.CamID == id && m.UseId == sessao);
            if (campanha == null)
            {
                //return NotFound();
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
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

        public async Task<IActionResult> VotosCampanha(int? id, DateTime? minDate, DateTime? maxDate)
        {
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
            var result = await _votosService.FindAllAsync(id, minDate, maxDate);
            return View(result);
        }
    }
}
