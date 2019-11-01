using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nota2.Data;
using Nota2.Models;

namespace Nota2.Controllers
{
    public class TipoUsuariosController : Controller
    {
        private readonly MyContext _context;
        public const string SessionKeyId = "_Id";
        public const string SessionKeyType = "_Type";

        public TipoUsuariosController(MyContext context)
        {
            _context = context;
        }

        // GET: TipoUsuarios
        public async Task<IActionResult> Index()
        {

            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (sessaoType != null)
            {
                if (!sessaoType.Equals("1"))
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


            return View(await _context.TipoUsuarios.ToListAsync());
                       
        }

        // GET: TipoUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            var tipoUsuario = await _context.TipoUsuarios
                .FirstOrDefaultAsync(m => m.TipoID == id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return View(tipoUsuario);
        }

        // GET: TipoUsuarios/Create
        public IActionResult Create()
        {
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                return NotFound();
            }

            return View();
        }

        // POST: TipoUsuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoID,Descricao")] TipoUsuario tipoUsuario)
        {
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(tipoUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoUsuario);
        }
        
        // GET: TipoUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            var tipoUsuario = await _context.TipoUsuarios.FindAsync(id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }
            return View(tipoUsuario);
        }

        // POST: TipoUsuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoID,Descricao")] TipoUsuario tipoUsuario)
        {
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoUsuarioExists(tipoUsuario.TipoID))
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
            return View(tipoUsuario);
        }

        // GET: TipoUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            var tipoUsuario = await _context.TipoUsuarios
                .FirstOrDefaultAsync(m => m.TipoID == id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return View(tipoUsuario);
        }

        // POST: TipoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoUsuario = await _context.TipoUsuarios.FindAsync(id);
            _context.TipoUsuarios.Remove(tipoUsuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoUsuarioExists(int id)
        {
            return _context.TipoUsuarios.Any(e => e.TipoID == id);
        }
    }
}
