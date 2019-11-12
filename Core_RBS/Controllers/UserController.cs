using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_RBS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;using Core_RBS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Core_RBS.Controllers
{
    [Authorize(Policy = "Administrador")]
    public class UserController : Controller
    {
        
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public UserController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}