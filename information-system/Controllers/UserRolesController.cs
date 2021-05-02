using information_system.Data;
using information_system.Enums;
using information_system.Models;
using information_system.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment _appEnvironment;
        public UserRolesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SystemContext systemContext, IWebHostEnvironment appEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _systemContext = systemContext;
            _appEnvironment = appEnvironment;
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
            List<UserRole> userRole = new List<UserRole>();
            foreach (User ur in user)
            {
                List<string> roles = await _userManager.GetRolesAsync(ur) as List<string>;
                string role = roles.Count > 0 ? roles[0] : "";
                userRole.Add(new UserRole() { user = ur, role = role });
            }
            return Json(userRole);
        }
        [HttpGet]
        public JsonResult ReturnBook()
        {
            List<Book> book = _systemContext.Books.Include(b=>b.Status)
                .Include(b=>b.BookGenres).ThenInclude(bk=>bk.Genre).ToList();
            return Json(book);
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
            EditUserViewModel model = new EditUserViewModel
            {
                Id = userId,
                Email = user.Email,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NumderReadTicket = user.NumderReadTicket,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
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
                    if (!string.IsNullOrEmpty(user.ProfilePicture))
                        System.IO.File.Delete(_appEnvironment.WebRootPath + user.ProfilePicture);
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
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
        public async Task<IActionResult> EditP(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            EditLoginPasswordViewModel model = new EditLoginPasswordViewModel
            {
                Id = userId,
                Email = user.Email,
                Username = user.UserName,

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditPas(EditLoginPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator =
                HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                        await _userManager.UpdateAsync(user);
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
        public IActionResult AddGenre()
        {
            return RedirectToAction("Index", "GenreManage");
        }
        public IActionResult AddBook()
        {
            return RedirectToAction("CreateBook");
        }
        public IActionResult CreateBook()
        {
            return View(new CreateBookViewModel());
        }
        public async Task<IActionResult> CreateB(CreateBookViewModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string[] _genre = (from t in JsonConvert.DeserializeObject<Dictionary<string, string>[]>(model.Genre) select t["value"]).ToArray();
                List<Genre> genres = _systemContext.Genres.Where(g => _genre.Contains(g.GenreName)).ToList();

                Book book = new Book
                {
                    Name = model.BookName,
                    Author = model.Author,
                    Articl = model.Articl,
                    Description = model.Description,
                    Year = model.Year,
                };

                Status status = _systemContext.Statuses.FirstOrDefault(c => c.StatusName.ToLower().Contains("в наличии"));
                if (status == null)
                    status = _systemContext.Statuses.First();

                book.Status = status;

                string path = "";
                if (!string.IsNullOrEmpty(book.BookPicture))
                    System.IO.File.Delete(_appEnvironment.WebRootPath + book.BookPicture);
                if (file != null)
                {
                    path = @"/files/book/" + book.Name + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                book.BookPicture = path;

                _systemContext.Add(book);
                
                foreach(Genre genre in genres)
                {
                    _systemContext.BookGenres.Add(new BookGenre() { Book = book, Genre = genre });
                }
                
                _systemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public IActionResult AddStatus()
        {
            return RedirectToAction("Index", "StatusManager");
        }
        public IActionResult EditBook(int bookId)
        {
            Book book = _systemContext.Books.Find(bookId);
            if (book == null)
            {
                return NotFound();
            }
            EditBookViewModel model = new EditBookViewModel
            {
                Id = bookId,
                BookName = book.Name,
                Author = book.Author,
                Articl = book.Articl,
                Description = book.Description,
                Year = book.Year,
                BookPicture = book.BookPicture
            };
            return View(model);
        }
        public async Task<IActionResult> EditB(EditBookViewModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Book book = _systemContext.Books.Find(model.Id);
                if (book != null)
                {
                    book.Name = model.BookName;
                    book.Author = model.Author;
                    book.Articl = model.Articl;
                    book.Description = model.Description;
                    book.Year = model.Year;

                    string path = model.BookPicture;
                    if (!string.IsNullOrEmpty(book.BookPicture))
                        System.IO.File.Delete(_appEnvironment.WebRootPath + book.BookPicture);
                    if (file != null)
                    {
                        path = @"/files/book/" + book.Name + Path.GetExtension(file.FileName);

                        using(var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    book.BookPicture = path;
                    _systemContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public IActionResult ManageG()
        {
            return RedirectToAction("ManageGenre");
        }
        public IActionResult ManageGenre()
        {
            return View();
        }
        public IActionResult ManageS()
        {
            return RedirectToAction("ManageStatus");
        }
        public IActionResult ManageStatus()
        {
            return View();
        }
        public IActionResult DeleteBook(int bookId)
        {
            Book book = _systemContext.Books.FirstOrDefault(b => b.Id == bookId);
            _systemContext.Entry(book).State = EntityState.Deleted;
            _systemContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult GetGenre()
        {
            return Json(_systemContext.Genres.ToArray());
        }
    }
}
