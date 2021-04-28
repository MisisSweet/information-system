using information_system.Data;
using information_system.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Controllers
{
    public class GenreManageController : Controller
    {
        private readonly SystemContext _systemContext;

        public GenreManageController(SystemContext systemContext)
        {
            _systemContext = systemContext;
        }

        public IActionResult Index()
        {
            var genre = _systemContext.Genres.ToList();
            return View(genre);
        }
        [HttpPost]
        public IActionResult AddGenre(string genreName)
        {
            if (genreName != null)
            {
                Genre newGenre = new Genre()
                {
                    GenreName = genreName
                };
                _systemContext.Genres.Add(newGenre);
            }
            _systemContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Back()
        {
            return RedirectToAction("Index", "UserRoles");
        }
    }
}
