using information_system.Data;
using information_system.Models;
using information_system.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly SystemContext _context;

        public HomeController(SystemContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Book> books = _context.Books.Skip(Math.Max(0, _context.Books.Count() - 5)).ToList();
            List<Work> works = _context.Works.ToList();
            return View(new HomeModel() { Books = books, Works = works});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
