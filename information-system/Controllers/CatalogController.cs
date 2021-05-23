using information_system.Data;
using information_system.Models;
using information_system.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Controllers
{
    public class CatalogController : Controller
    {
        private SystemContext _context;

        public CatalogController(SystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetBooks(FilterModel model)
        {
            const int countOnPage = 10;

            string[] _genre = new string[0];
            string[] _specialty = new string[0];
            string[] _discipline = new string[0];


            List<Book> books = _context.Books
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.BookDiscs)
                    .ThenInclude(bd => bd.Discipline)
                .Include(b => b.BookSpecs).
                    ThenInclude(bs => bs.Specialty).ToList();

            if (!string.IsNullOrEmpty(model.Author))
            {
                books = books.Where(b => b.Author.Equals(model.Author)).ToList();
            }
            if (!string.IsNullOrEmpty(model.Genre))
            {
                _genre = (from t in JsonConvert.DeserializeObject<Dictionary<string, string>[]>(model.Genre) select t["value"]).ToArray();
                books = books.Where(b => isContains(b.BookGenres.Select(c=>c.Genre.GenreName).ToArray(), _genre)).ToList();
            }
            if (!string.IsNullOrEmpty(model.Discipline))
            {
                _discipline = (from d in JsonConvert.DeserializeObject<Dictionary<string, string>[]>(model.Discipline) select d["value"]).ToArray();
                books = books.Where(b => isContains(b.BookDiscs.Select(c => c.Discipline.NameDiscipline).ToArray(), _discipline)).ToList();
            }
            if (!string.IsNullOrEmpty(model.Specialty))
            {
                _specialty = (from s in JsonConvert.DeserializeObject<Dictionary<string, string>[]>(model.Specialty) select s["value"]).ToArray();
                books = books.Where(b => isContains(b.BookSpecs.Select(c => c.Specialty.NameSpecialty).ToArray(), _specialty)).ToList();
            }

            model.Books = books;
            model.CountPage = (books.Count / countOnPage) + (books.Count % countOnPage > 0 ? 1 : 0);

            model.Books = model.Books.Skip(countOnPage * (model.CurrentPage)).Take(countOnPage).ToList();
            return Json(model);
        }
        private bool isContains(string[] array1, string[] array2)
        {
            foreach (string g in array2)
            {
                if (!array1.Contains(g))
                {
                    return false;
                }
            }

            return true;

        }

        public IActionResult Book(int id)
        {
            Book b = _context.Books.Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.BookDiscs)
                    .ThenInclude(bd => bd.Discipline)
                .Include(b => b.BookSpecs).
                    ThenInclude(bs => bs.Specialty)
                .Include(b => b.Chapters)
                    .FirstOrDefault(b => b.Id == id);

            List<Chapter> c = b.Chapters.ToList();
            c.Sort(delegate (Chapter x, Chapter y)
            {
                return x.NumberChapter-y.NumberChapter;
            });
            b.Chapters = c;
            return View(b);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddLoan(int idBook, string idUser)
        {
            Status status = _context.Statuses.FirstOrDefault(s => s.StatusName.ToLower().Equals("в обработке"));
            Loan loan = _context.Loans.FirstOrDefault(t => t.UserId == idUser && t.BookId == idBook && t.StatusId == status.Id);
            if (loan == null)
            {
                _context.Loans.Add(new Loan()
                {
                    BookId = idBook,
                    UserId = idUser,
                    Status = status,
                    Date = DateTime.Now
                });
                _context.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
    }
}
