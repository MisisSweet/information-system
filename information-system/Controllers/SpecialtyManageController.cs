using information_system.Data;
using information_system.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Controllers
{
    public class SpecialtyManageController : Controller
    {
        private readonly SystemContext _systemContext;

        public SpecialtyManageController(SystemContext systemContext)
        {
            _systemContext = systemContext;
        }
        public IActionResult Index()
        {
            var spec = _systemContext.Specialties.ToList();
            return View(spec);
        }
        [HttpPost]
        public IActionResult AddSpecialty(string specName)
        {
            if (specName != null)
            {
                Specialty newSpecialty = new Specialty()
                {
                    NameSpecialty = specName
                };
                _systemContext.Specialties.Add(newSpecialty);
            }
            _systemContext.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}
