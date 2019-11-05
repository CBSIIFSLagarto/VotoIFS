using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core_RBS.Data;
using Core_RBS.Models;
using Core_RBS.Util;


namespace Nota2.Controllers
{
    public class VotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VotosController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: Votoes/Votar
        public IActionResult Votar(string id)
        {
            var campanha = _context.Campanhas.Where(p => p.Chave.Equals(id)).FirstOrDefault();
            if (campanha != null)
            {
                if (DateTime.Compare(campanha.DataHoraInicio,DateTime.Now) <= 0 && DateTime.Compare(campanha.DataHoraFim,DateTime.Now) >= 0)
                {
                    return View();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }            
        }
        // POST: Votoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Votar(string id, [Bind("VotID,Nota,Comentario,CamId")] Voto voto)
        {
            if (ModelState.IsValid)
            {
                Campanha campanha = _context.Campanhas.Where(p => p.Chave == id).FirstOrDefault();
                if (campanha != null)
                {
                    voto.CamId = campanha.CamID;

                    var chave = SHA.GenerateSHA256String(Convert.ToString(voto.CamId));
                    var valor = SHA.GenerateSHA256String(Convert.ToString(voto.Nota));
                    //PEGANDO COOKIE
                    string cookieValueFromReq = Request.Cookies[chave];

                    if (cookieValueFromReq == null)
                    {
                        //GERANDO COOKIE
                        this.Set(chave, valor, 10);
                        voto.DataVoto = DateTime.Now;
                        _context.Add(voto);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    return NotFound();
                }

            }
            ViewData["CamId"] = new SelectList(_context.Campanhas, "CamID", "Chave", voto.CamId);
            return View(voto);
        }

        private bool VotoExists(long id)
        {
            return _context.Votos.Any(e => e.VotID == id);
        }


        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
    }
}
