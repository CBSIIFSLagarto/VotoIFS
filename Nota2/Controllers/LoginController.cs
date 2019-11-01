using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nota2.Data;
using Nota2.ModelsView;

namespace Nota2.Controllers
{
    public class LoginController : Controller
    {
        private readonly MyContext _context;
        public const string SessionKeyId = "_Id";
        public const string SessionKeyName = "_Name";
        public const string SessionKeyType = "_Type";

        public LoginController(MyContext context)
        {
            _context = context;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logoff()
        {
            EraseInSession();
            return RedirectToAction("index","Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,senha")] Login login)
        {
            if (ModelState.IsValid)
            {

                var result = _context.Usuarios.Where(p => p.Email.Equals(login.Email) && p.senha.Equals(login.senha)).FirstOrDefault();
                if (result != null)
                {
                    RecordInSession(SessionKeyId, Convert.ToString(result.UseID));
                    RecordInSession(SessionKeyName, result.Nome);
                    RecordInSession(SessionKeyType, Convert.ToString(result.TipoId));
                    return RedirectToAction("Index", "Home");
                }

            }
            return View(login);
        }

        private void RecordInSession(string name, string action)
        {            
            HttpContext.Session.SetString(name, action);
        }
        private void EraseInSession()
        {            
            HttpContext.Session.Clear();
        }
    }
}