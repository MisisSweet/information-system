using information_system.Data;
using information_system.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Controllers
{
    public class StatusManagerController : Controller
    {
        private readonly SystemContext _systemContext;

        public StatusManagerController(SystemContext systemContext)
        {
            _systemContext = systemContext;
        }

        public IActionResult Index()
        {
            var genre = _systemContext.Statuses.ToList();
            return View(genre);
        }
        [HttpPost]
        public IActionResult AddStatus(string statusName)
        {
            if (statusName != null)
            {
                Status newStatus = new Status()
                {
                    StatusName = statusName
                };
                _systemContext.Statuses.Add(newStatus);
            }
            _systemContext.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}
