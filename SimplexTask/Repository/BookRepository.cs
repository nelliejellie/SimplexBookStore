using Microsoft.EntityFrameworkCore;
using SimplexTask.DataAccess;
using SimplexTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Repository
{
    public class BookRepository
    {
        private readonly AppDbContext _database;

        public BookRepository(AppDbContext database)
        {
            _database = database;
        }

        public async Task<List<Book>> AllBooks()
        {
            var books = await _database.Books.ToListAsync();

            return books;
        }
        public async Task<List<Book>> AllBooksByUser(string id)
        {
            var books = await _database.Books.Where(b => b.UserId == id).ToListAsync();

            return books;
        }
        public async Task<bool> AddBook(Book book)
        {
            await _database.Books.AddAsync(book);

            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }
        public async Task<bool> RemoveBook(Guid id)
        {
            var book = await _database.Books.FindAsync(id);
            _database.Books.Remove(book);

            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> UpdateBookRating(string bookrating, Guid Id)
        {
            var updateBook = await _database.Books.FirstOrDefaultAsync(Book => Book.Id == Id);

            updateBook.Rating = bookrating;

            var changes = await _database.SaveChangesAsync();

            return changes > 0;

        }

        public async Task<List<Book>> FilterBooksByRating(string bookrating, string id)
        {
            var books = await _database.Books.Where(book => book.Rating == bookrating).Where(book => book.UserId == id).ToListAsync();

            return books;
        }
    }
}
