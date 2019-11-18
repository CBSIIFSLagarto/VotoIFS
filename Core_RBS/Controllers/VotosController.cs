using Core_RBS.Data;
using Core_RBS.Models;
using Core_RBS.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Core_RBS.Controllers
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
            string msg = "";
            var campanha = _context.Campanhas.Where(p => p.Chave.Equals(id)).FirstOrDefault();
            if (campanha != null)
            {
                if (DateTime.Compare(campanha.DataHoraInicio, DateTime.Now) <= 0)
                {
                    if (DateTime.Compare(campanha.DataHoraFim, DateTime.Now) >= 0)
                    {
                        ViewBag.CampanhaDescricao = campanha.Descricao;
                        return View();
                    }
                    else
                    {
                        msg = "A votação encerrou-se às " + campanha.DataHoraFim.ToString("HH:mm") + " do dia " + campanha.DataHoraFim.ToString("dd/MM/yyyy") + ". Não é possível mais votar.";
                    }
                }
                else
                {
                    msg = "A votação iniciará às " + campanha.DataHoraInicio.ToString("HH:mm") + " do dia " + campanha.DataHoraInicio.ToString("dd/MM/yyyy") + ". Favor, aguarde.";
                }
                
            }
            else
            {
                msg = "A chave de acesso informada é inválida!";
                
            }
            ViewBag.MSG = msg;
            //return RedirectToAction(nameof(Index));
            return View("Index");
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
                string msg = "";
                Campanha campanha = _context.Campanhas.Where(p => p.Chave == id).FirstOrDefault();
                if (campanha != null)
                {
                    if (DateTime.Compare(DateTime.Now, campanha.DataHoraFim) < 0)
                    {
                        voto.CamId = campanha.CamID;

                        var chave = SHA.GenerateSHA256String(Convert.ToString(voto.CamId));
                        var valor = SHA.GenerateSHA256String(Convert.ToString(voto.Nota));
                        //PEGANDO COOKIE
                        string cookieValueFromReq = Request.Cookies[chave];

                        msg = "Voto registrado com sucesso!";
                        ViewBag.MSG = msg;
                        if (cookieValueFromReq == null)
                        {
                            //GERANDO COOKIE
                            this.Set(chave, valor, 10);
                            voto.DataVoto = DateTime.Now;
                            _context.Add(voto);
                            await _context.SaveChangesAsync();
                            //return RedirectToAction(nameof(Index));
                            return View("Index");
                        }
                        else
                        {
                            //return RedirectToAction(nameof(Index));
                            return View("Index");
                        }
                    }
                    else
                    {
                        msg = "O período de votação dessa campanha expirou.";
                    }
                }
                else
                {
                    return NotFound();
                }
                ViewBag.MSG = msg;
                return View("Index");
            }
            ViewData["CamId"] = new SelectList(_context.Campanhas, "CamID", "Chave", voto.CamId);
            return View(voto);
        }

        [Authorize(Policy = "Professor")]
        public IActionResult VotosPorCampanha(int id)
        {
            ViewBag.DescricaoCampanha = _context.Campanhas.Where(p => p.CamID == id).FirstOrDefault().Descricao;
            var votos = _context.Votos.Where(p => p.CamId == id).ToList();
            if (votos.Count != 0)
            {
                ViewBag.Media = Math.Round(votos.Sum(p => p.Nota) / (double)votos.Count, 2);
            }
            else
            {
                ViewBag.Media = 0;
            }
            
            return View(votos);
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
