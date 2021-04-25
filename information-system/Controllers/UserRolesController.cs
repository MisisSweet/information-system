using information_system.Data;
using information_system.Enums;
using information_system.Models;
using information_system.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SystemContext _systemContext;
        public UserRolesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SystemContext systemContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _systemContext = systemContext;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
       
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public IActionResult AddRole()
        {
            return Redirect("~/RoleManager/Index");
        }
        [HttpGet]
        public async Task<JsonResult> ReturnUser()
        {
            List<User> user = _systemContext.Users.ToList();
            List < UserRole > userRole= new List<UserRole>();
            foreach (User ur in user)
            {
                List<string> roles = await _userManager.GetRolesAsync(ur) as List<string>;
                string role = roles.Count > 0 ? roles[0] : "";
                userRole.Add(new UserRole() { user = ur, role = role });
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
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User {
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    NumderReadTicket=model.NumderReadTicket
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
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
        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { 
                Id=userId,
                Email=user.Email,
                Username=user.UserName,
                FirstName=user.FirstName,
                LastName=user.LastName,
                NumderReadTicket=user.NumderReadTicket,
                PhoneNumber=user.PhoneNumber,
                ProfilePicture=user.ProfilePicture
            };
            return View(model);
        }
        [HttpGet]
        public JsonResult GetAllRole()
        {
            string[] types = Enum.GetNames(typeof(Roles));
            int[] values = Enum.GetValues(typeof(Roles)) as int[];
            Dictionary<string, int> result = new Dictionary<string, int>();
            for (int i = 0; i < types.Length; i++)
            {
                result.Add(types[i], values[i]);
            }
            return Json(result);
        }
        public IActionResult Back()
        {
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(EditUserViewModel model)
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
                    user.ProfilePicture = model.ProfilePicture;

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
