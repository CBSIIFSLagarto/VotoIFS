﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;using Core_RBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_RBS.Controllers
{
    [Authorize(Policy = "Administrador")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> roleManager;

        /// 
        /// Injecting Role Manager
        /// 
        /// 
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
    }
}