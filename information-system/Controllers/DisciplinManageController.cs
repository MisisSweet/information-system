using information_system.Data;
using information_system.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Controllers
{
    public class DisciplinManageController : Controller
    {
        private readonly SystemContext _systemContext;

        public DisciplinManageController(SystemContext systemContext)
        {
            _systemContext = systemContext;
        }
        public IActionResult Index()
        {
            var disc = _systemContext.Disciplines.ToList();
            return View(disc);
        }
        [HttpPost]
        public IActionResult AddDiscipline(string discName)
        {
            if (discName != null)
            {
                Discipline newDiscipline = new Discipline()
                {
                    NameDiscipline = discName
                };
                _systemContext.Disciplines.Add(newDiscipline);
            }
            _systemContext.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}
