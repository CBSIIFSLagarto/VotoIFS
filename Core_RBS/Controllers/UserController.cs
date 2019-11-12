using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Core_RBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_RBS.Controllers
{
    [Authorize(Policy = "Administrador")]
    public class UserController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;        
        public UserController(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }
        // GET: Campanhas        
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        public async Task<IActionResult> Reset(string id)
        {
            Usuario user = await _userManager.FindByNameAsync(id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, "ifs@123");
            if (result.Succeeded)
            {
                TempData["Success"] = "Senha resetada com sucesso, nova senha: ifs@123";
            }
            else
            {
                TempData["Success"] = "Ocorreu algum erro, entrar contato com administrador.";
            }

            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> RoleUpdate(string id)
        {
            Usuario user = await _userManager.FindByNameAsync(id);
            var role = _roleManager.FindByNameAsync("Administrador").Result;
            if (role != null)
            {
                IdentityResult result;
                var isAdmin = await _userManager.IsInRoleAsync(user, role.Name);
                if (isAdmin)
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Você removeu o usuário "+ user.Nome +" do grupo de "+role.Name;
                    }
                    else
                    {
                        TempData["Success"] = "Ocorreu algum erro, entrar contato com administrador.";
                    }
                }
                else
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Você adicionou o usuário " + user.Nome + " ao grupo de " + role.Name;
                    }
                    else
                    {
                        TempData["Success"] = "Ocorreu algum erro, entrar contato com administrador.";
                    }
                }
            }

            return RedirectToAction("Index", "User");
        }
    }
}