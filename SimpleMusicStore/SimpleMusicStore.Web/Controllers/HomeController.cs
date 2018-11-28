﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleMusicStore.Data;
using SimpleMusicStore.Models;
using SimpleMusicStore.Web.Areas.Admin.Services;
using SimpleMusicStore.Web.Models;

namespace SimpleMusicStore.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(
           UserManager<SimpleUser> userManager,
           SignInManager<SimpleUser> signInManager,
           SimpleDbContext context,
           RoleManager<IdentityRole> roleManager
           )
            : base(userManager, signInManager, context, roleManager)
        {
        }
        public IActionResult Index()
        {

            service.ImportFromDiscogs("https://www.discogs.com/Soul-Capsule-Overcome/master/484910");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
