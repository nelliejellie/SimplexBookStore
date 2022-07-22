using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplexTask.Models;
using SimplexTask.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SimplexTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookRepository _bookRepository;

        public HomeController(ILogger<HomeController> logger, BookRepository bookRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var books = await _bookRepository.AllBooksByUser(userId);
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                var allBooks = await _bookRepository.AllBooks();
                ViewBag.Books = allBooks;
                return View();
            }
            ViewBag.Books = books;
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(string Rating, string? CreatedOn)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var books = await _bookRepository.AllBooksByUser(userId);
            var updatedBooks = await _bookRepository.FilterBooksByRating(Rating, userId);
            if(Rating == "all")
            {
                ViewBag.Books = books;
            }
            else
            {
                ViewBag.Books = updatedBooks;
            }
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                var filterValue = Convert.ToDateTime(CreatedOn).ToString("MM dd yyyy");
                var allBooks = await _bookRepository.AllBooks();
                var filters = allBooks.Where(book => book.CreatedOn.ToString("MM dd yyyy") == filterValue).ToList();
                ViewBag.Books = filters;
                return View();
            }
            return View();
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            book.UserId = userId;
            book.UserName = User.Identity.Name;
            var newbook = await _bookRepository.AddBook(book);
            TempData["BookAdded"] = "your book has been added successfully";
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            
            try
            {
                var isDeleted = await _bookRepository.RemoveBook(Id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string Rating, Guid Id)
        {
            try
            {
                var isUpdated = await _bookRepository.UpdateBookRating(Rating, Id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

        }

       
    }
}
