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
    [Authorize(Roles = "SuperAdmin,Employee")]
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
        public IActionResult Index()
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
            var model = new ManageUserRolesViewModel();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModelItem
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.SelectedValue = role.Name;
                }

                model.List.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(ManageUserRolesViewModel model, string userId)
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
            result = await _userManager.AddToRoleAsync(user, model.SelectedValue);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
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
            List<Book> book = _systemContext.Books
                .Include(b => b.BookGenres)
                    .ThenInclude(bk => bk.Genre)
                .Include(b => b.BookDiscs)
                    .ThenInclude(bd => bd.Discipline)
                .Include(b => b.BookSpecs)
                    .ThenInclude(bs => bs.Specialty)
                .ToList();
            return Json(book);
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
                    NumderReadTicket = model.NumderReadTicket,
                    GroupNumber = model.GroupNumber
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
                ProfilePicture = user.ProfilePicture,
                GroupNumber = user.GroupNumber
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
        [HttpPost]
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
                    user.GroupNumber = model.GroupNumber;

                    string path = user.ProfilePicture;

                    if (!string.IsNullOrEmpty(user.ProfilePicture) && file != null)
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + user.ProfilePicture);
                        path = "";
                    }

                    if (file != null)
                    {
                        path = @"/files/img/" + user.UserName + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }

                    user.ProfilePicture = path;

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
        public IActionResult CreateBook()
        {
            return View(new CreateBookViewModel());
        }
        public async Task<IActionResult> CreateB(CreateBookViewModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string[] _genre = (from t in JsonConvert.DeserializeObject<Dictionary<string, string>[]>(model.Genre) select t["value"]).ToArray();
                string[] _specialty = (from s in JsonConvert.DeserializeObject<Dictionary<string, string>[]>(model.Specialty) select s["value"]).ToArray();
                string[] _discipline = (from d in JsonConvert.DeserializeObject<Dictionary<string, string>[]>(model.Discipline) select d["value"]).ToArray();
                List<Genre> genres = _systemContext.Genres.Where(g => _genre.Contains(g.GenreName)).ToList();
                List<Specialty> specialties = _systemContext.Specialties.Where(s => _specialty.Contains(s.NameSpecialty)).ToList();
                List<Discipline> disciplines = _systemContext.Disciplines.Where(d => _discipline.Contains(d.NameDiscipline)).ToList();

                Book book = new Book
                {
                    Name = model.BookName,
                    Author = model.Author,
                    Articl = model.Articl,
                    Description = model.Description,
                    Year = model.Year,
                    Count = model.Count,
                };

                List<Chapter> chapters = new List<Chapter>();
                for (int i = 0; i < model.Chapters.Count; i++)
                {
                    chapters.Add(new Chapter()
                    {
                        Book = book,
                        Name = model.Chapters[i],
                        NumberChapter = i
                    });
                }
                book.Chapters = chapters;


                string path = "";
                if (!string.IsNullOrEmpty(book.BookPicture))
                    System.IO.File.Delete(_appEnvironment.WebRootPath + book.BookPicture);
                if (file != null)
                {
                    path = @"/files/book/" + book.Name + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                book.BookPicture = path;

                _systemContext.Add(book);

                foreach (Genre genre in genres)
                {
                    _systemContext.BookGenres.Add(new BookGenre() { Book = book, Genre = genre });
                }
                foreach (Discipline disc in disciplines)
                {
                    _systemContext.BookDiscs.Add(new BookDisc() { Book = book, Discipline = disc });
                }
                foreach (Specialty spec in specialties)
                {
                    _systemContext.BookSpecs.Add(new BookSpec() { Book = book, Specialty = spec });
                }
                _systemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public IActionResult EditBook(int bookId)
        {
            Book book = _systemContext.Books.Include(b => b.Chapters).FirstOrDefault(b => b.Id == bookId);
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
                BookPicture = book.BookPicture,
                Count = book.Count,
                Chapters = book.Chapters.Select(c => c.Name).ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> EditB(EditBookViewModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Book book = _systemContext.Books.Include(b => b.Chapters).FirstOrDefault(b => b.Id == model.Id);
                if (book != null)
                {
                    book.Name = model.BookName;
                    book.Author = model.Author;
                    book.Articl = model.Articl;
                    book.Description = model.Description;
                    book.Year = model.Year;
                    book.Count = model.Count;

                    string path = book.BookPicture;
                    if (!string.IsNullOrEmpty(book.BookPicture) && file != null)
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + book.BookPicture);
                        path = "";
                    }
                    if (file != null)
                    {
                        path = @"/files/book/" + book.Name + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(file.FileName);

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    book.BookPicture = path;

                    bool isDelete = book.Chapters.Count > model.Chapters.Count;
                    bool isAdd = model.Chapters.Count > book.Chapters.Count;
                    int count = Math.Abs(model.Chapters.Count - book.Chapters.Count);
                    int length = Math.Min(model.Chapters.Count, book.Chapters.Count);

                    for (int i = 0; i < length; i++)
                    {
                        book.Chapters.ToList()[i].Name = model.Chapters[i];
                    }
                    if (isDelete)
                    {
                        for (int i = length; i < length + count; i++)
                        {
                            Chapter c = book.Chapters.ToList()[i];
                            _systemContext.Entry(c).State = EntityState.Deleted;

                        }
                    }
                    if (isAdd)
                    {
                        for (int i = length; i < length + count; i++)
                        {
                            book.Chapters.Add(new Chapter()
                            {
                                Book = book,
                                Name = model.Chapters[i],
                                NumberChapter = i
                            });
                        }
                    }
                    _systemContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public IActionResult ManageGenre(int bookId)
        {
            ViewBag.bookId = bookId;
            var book = _systemContext.Books.Include(g => g.BookGenres).ThenInclude(g => g.Genre).FirstOrDefault(c => c.Id == bookId);
            if (book == null)
            {
                ViewBag.ErrorMessage = $"Book with Id = {bookId} cannot be found";
                return View("NotFound");
            }
            ViewBag.BookName = book.Name;
            var model = new List<ManageGenreViewModel>();
            List<String> genres = book.BookGenres.Select(c => c.Genre.GenreName).ToList();
            foreach (var genre in _systemContext.Genres)
            {
                var manageGenreViewModel = new ManageGenreViewModel
                {
                    GenreId = genre.Id,
                    GenreName = genre.GenreName
                };
                if (genres.Contains(genre.GenreName))
                {
                    manageGenreViewModel.Selected = true;
                }
                else
                {
                    manageGenreViewModel.Selected = false;
                }
                model.Add(manageGenreViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult ManageGenre(List<ManageGenreViewModel> model, int bookId)
        {
            var book = _systemContext.Books.Include(g => g.BookGenres).ThenInclude(g => g.Genre).FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return View();
            }
            List<String> genres = book.BookGenres.Select(c => c.Genre.GenreName).ToList();
            foreach (BookGenre bookGenre in book.BookGenres)
            {
                _systemContext.Entry(bookGenre).State = EntityState.Deleted;
            }
            var gen = model.Where(x => x.Selected).ToList();
            foreach (var g in gen)
            {
                BookGenre newBookGenre = new BookGenre
                {
                    Book = book,
                    GenreId = g.GenreId
                };
                _systemContext.BookGenres.Add(newBookGenre);
            }
            _systemContext.SaveChanges();
            return RedirectToAction("EditBook", new { bookId = bookId });
        }
        public IActionResult ManageStatus(int loanId)
        {
            ViewBag.loanId = loanId;
            var loan = _systemContext.Loans.Include(c => c.Status).FirstOrDefault(c => c.Id == loanId);
            if (loan == null)
            {
                ViewBag.ErrorMessage = $"Loan with Id = {loanId} cannot be found";
                return View("NotFound");
            }
            var model = new ManageStatusViewModel();

            foreach (var status in _systemContext.Statuses)
            {
                if (status.StatusName.Equals(loan.Status.StatusName))
                    model.SelectedValue = status.StatusName;
                model.List.Add(status);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult ManageStatus(ManageStatusViewModel model, int loanId)
        {
            var loan = _systemContext.Loans.FirstOrDefault(b => b.Id == loanId);
            if (loan == null)
            {
                return View();
            }
            var stat = model.List.FirstOrDefault(c => c.StatusName.Equals(model.SelectedValue));
            loan.Status = stat;
            _systemContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult ManageDiscipline(int bookId)
        {
            ViewBag.bookId = bookId;
            var book = _systemContext.Books.Include(d => d.BookDiscs).ThenInclude(db => db.Discipline).FirstOrDefault(c => c.Id == bookId);
            if (book == null)
            {
                ViewBag.ErrorMessage = $"Book with Id = {bookId} cannot be found";
                return View("NotFound");
            }
            ViewBag.BookName = book.Name;
            var model = new List<ManageDisciplineViewModel>();
            List<String> disciplines = book.BookDiscs.Select(c => c.Discipline.NameDiscipline).ToList();
            foreach (var discipline in _systemContext.Disciplines)
            {
                var manageDisciplineViewModel = new ManageDisciplineViewModel
                {
                    DisciplineId = discipline.Id,
                    DisciplineName = discipline.NameDiscipline,
                };
                if (disciplines.Contains(discipline.NameDiscipline))
                {
                    manageDisciplineViewModel.Selected = true;
                }
                else
                {
                    manageDisciplineViewModel.Selected = false;
                }
                model.Add(manageDisciplineViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult ManageDiscipline(List<ManageDisciplineViewModel> model, int bookId)
        {
            var book = _systemContext.Books.Include(g => g.BookDiscs).ThenInclude(g => g.Discipline).FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return View();
            }
            List<String> disciplines = book.BookDiscs.Select(c => c.Discipline.NameDiscipline).ToList();
            foreach (BookDisc bookDisc in book.BookDiscs)
            {
                _systemContext.Entry(bookDisc).State = EntityState.Deleted;
            }
            var disc = model.Where(x => x.Selected).ToList();
            foreach (var d in disc)
            {
                BookDisc newBookDisc = new BookDisc
                {
                    Book = book,
                    DisciplineId = d.DisciplineId
                };
                _systemContext.BookDiscs.Add(newBookDisc);
            }
            _systemContext.SaveChanges();
            return RedirectToAction("EditBook", new { bookId = bookId });
        }
        public IActionResult ManageSpecialty(int bookId)
        {
            ViewBag.bookId = bookId;
            var book = _systemContext.Books.Include(b => b.BookSpecs).ThenInclude(s => s.Specialty).FirstOrDefault(c => c.Id == bookId);
            if (book == null)
            {
                ViewBag.ErrorMessage = $"Book with Id = {bookId} cannot be found";
                return View("NotFound");
            }
            ViewBag.BookName = book.Name;
            var model = new List<ManageSpecialtyViewModel>();
            List<String> specialties = book.BookSpecs.Select(c => c.Specialty.NameSpecialty).ToList();
            foreach (var specialty in _systemContext.Specialties)
            {
                var manageSpecialtyViewModel = new ManageSpecialtyViewModel
                {
                    SpecialtyId = specialty.Id,
                    SpecialtyName = specialty.NameSpecialty,
                };
                if (specialties.Contains(specialty.NameSpecialty))
                {
                    manageSpecialtyViewModel.Selected = true;
                }
                else
                {
                    manageSpecialtyViewModel.Selected = false;
                }
                model.Add(manageSpecialtyViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult ManageSpecialty(List<ManageSpecialtyViewModel> model, int bookId)
        {
            var book = _systemContext.Books.Include(g => g.BookSpecs).ThenInclude(g => g.Specialty).FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return View();
            }
            List<String> specialties = book.BookSpecs.Select(c => c.Specialty.NameSpecialty).ToList();
            foreach (BookSpec bookSpec in book.BookSpecs)
            {
                _systemContext.Entry(bookSpec).State = EntityState.Deleted;
            }
            var spec = model.Where(x => x.Selected).ToList();
            foreach (var s in spec)
            {
                BookSpec newBookSpec = new BookSpec
                {
                    Book = book,
                    SpecialtyId = s.SpecialtyId
                };
                _systemContext.BookSpecs.Add(newBookSpec);
            }
            _systemContext.SaveChanges();
            return RedirectToAction("EditBook", new { bookId = bookId });
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
        [HttpGet]
        public JsonResult GetDiscipline()
        {
            return Json(_systemContext.Disciplines.ToArray());
        }
        [HttpGet]
        public JsonResult GetSpecialty()
        {
            return Json(_systemContext.Specialties.ToArray());
        }
        [HttpGet]
        public JsonResult GetLoan()
        {
            List<Loan> result = _systemContext.Loans
                .Include(u => u.User)
                .Include(b => b.Book)
                .Include(s => s.Status).ToList();
            return Json(result);
        }
        public JsonResult GetLoanByUserId(string userId)
        {
            List<Loan> result = _systemContext.Loans
                .Include(b => b.Book)
                .Include(s => s.Status).Where(l => l.UserId.Equals(userId)).ToList();
            return Json(result);
        }
        [HttpGet]
        public IActionResult DeleteLoan(int loanId)
        {
            Loan loan = _systemContext.Loans.Include(l => l.Status).Include(l => l.Book).FirstOrDefault(l => l.Id == loanId);
            Status old = loan.Status;
            loan.Status = _systemContext.Statuses.FirstOrDefault(s => s.StatusName.ToLower().Equals("отменена"));
            if (old.StatusName.ToLower().Equals("в пользовании"))
                loan.Book.Count++;
            _systemContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AcceptLoan(int loanId)
        {
            Loan loan = _systemContext.Loans.Include(l => l.Status).Include(l => l.Book).FirstOrDefault(l => l.Id == loanId);
            if (loan.Book.Count > 0)
            {
                loan.Status = _systemContext.Statuses.FirstOrDefault(s => s.StatusName.ToLower().Equals("в пользовании"));
                loan.Book.Count--;
                _systemContext.SaveChanges();
            }


            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult ReturnLoan(int loanId)
        {
            Loan loan = _systemContext.Loans.Include(l => l.Status).Include(l => l.Book).FirstOrDefault(l => l.Id == loanId);
            Status old = loan.Status;
            loan.Status = _systemContext.Statuses.FirstOrDefault(s => s.StatusName.ToLower().Equals("возвращено"));
            if(old.StatusName.ToLower().Equals("в пользовании"))
                loan.Book.Count++;
            _systemContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
