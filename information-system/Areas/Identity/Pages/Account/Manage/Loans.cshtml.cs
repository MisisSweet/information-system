using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using information_system.Data;
using information_system.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace information_system.Areas.Identity.Pages.Account.Manage
{
    public class LoanModel : PageModel
    {
        public LoanModel(UserManager<User> userManager, SystemContext context)
        {
        }

        public void OnGet()
        {
        }
    }
}
