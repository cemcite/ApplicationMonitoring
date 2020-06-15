﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplicationUI.Models;

namespace WebApplicationUI.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
