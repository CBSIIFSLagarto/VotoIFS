using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nota2.Data;
using Nota2.Models;

namespace Nota2.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MyContext _context;
        public const string SessionKeyId = "_Id";
        public const string SessionKeyType = "_Type";

        

        public UsuariosController(MyContext context)
        {
            _context = context;

        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var sessaoId = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (sessaoId != 0)
            {                 

                if (sessaoType.Equals("1"))
                {
                    var myContext = _context.Usuarios.Include(u => u.TipoUsuario);
                    return View(await myContext.ToListAsync());
                }
                else
                {
                    var myContext = _context.Usuarios.Where(u => u.UseID.Equals(sessaoId));
                    return View(await myContext.ToListAsync());
                }                
                
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var sessaoId = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                if (sessaoId != id)
                {
                    return NotFound();
                }
                
            }
            
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(m => m.UseID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);            
            if (!sessaoType.Equals("1"))
            {
                return NotFound();
            }

            ViewData["TipoId"] = new SelectList(_context.TipoUsuarios, "TipoID", "Descricao");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UseID,Nome,Email,senha,TipoId")] Usuario usuario)
        {
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoId"] = new SelectList(_context.TipoUsuarios, "TipoID", "Descricao", usuario.TipoId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var sessaoId = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                if (sessaoId != id)
                {
                    return NotFound();
                }

            }

            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["TipoId"] = new SelectList(_context.TipoUsuarios, "TipoID", "Descricao", usuario.TipoId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UseID,Nome,Email,senha,TipoId")] Usuario usuario)
        {

            var sessaoId = Convert.ToInt32(HttpContext.Session.GetString(SessionKeyId));
            var sessaoType = HttpContext.Session.GetString(SessionKeyType);
            if (!sessaoType.Equals("1"))
            {
                if (sessaoId != id)
                {
                    return NotFound();
                }

            }

            if (id != usuario.UseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UseID))
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
            ViewData["TipoId"] = new SelectList(_context.TipoUsuarios, "TipoID", "Descricao", usuario.TipoId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
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

            var usuario = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(m => m.UseID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UseID == id);
        }
    }
}
