using information_system.Data;
using information_system.Enums;
using information_system.Models;
using information_system.Models.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Controllers
{
    public class EmployeePanel : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SystemContext _systemContext;
        private readonly IWebHostEnvironment _appEnvironment;
        public EmployeePanel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SystemContext systemContext, IWebHostEnvironment appEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _systemContext = systemContext;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> ReturnUser()
        {
            List<User> user = _systemContext.Users.ToList();
            List<UserRole> userRole = new List<UserRole>();
            foreach (User ur in user)
            {
                if(await _userManager.IsInRoleAsync(ur, Roles.Student.ToString()))
                {
                    userRole.Add(new UserRole() { user = ur, role = Roles.Student.ToString() });
                }
            }
            return Json(userRole);
        }
        public IActionResult AddUser()
        {
            return RedirectToAction("CreateUser");
        }
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model, string role)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    NumderReadTicket = model.NumderReadTicket
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> EditUser(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NumderReadTicket = user.NumderReadTicket,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
            };
            return View(model);
        }
        public IActionResult Back()
        {
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(EditUserViewModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Username;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.NumderReadTicket = model.NumderReadTicket;
                    user.PhoneNumber = model.PhoneNumber;

                    string path = @"/files/img/" + user.UserName + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                    {
                        if (!string.IsNullOrEmpty(user.ProfilePicture))
                            System.IO.File.Delete(_appEnvironment.WebRootPath + user.ProfilePicture);
                        await file.CopyToAsync(fileStream);
                        user.ProfilePicture = path;
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            //TODO: в будущем доработать
            User user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}
