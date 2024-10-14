using DotNect_SQL.Data;
using DotNect_SQL.Entities;
using DotNect_SQL.WebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNect_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DataContext _context;

        public BookController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBooks() {
            var books = await _context.Books.Include(book => book.BorrowPeople)
                .ThenInclude(people => people.People)
                .ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if(book is null)
                return NotFound("Book not found.");
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> AddBook(CreateBookRequest book)
        {
            _context.Books.Add(new Book { 
                Id = book.Id,
                Name = book.Name,
                AuthorId = book.AuthorId,
            });
            await _context.SaveChangesAsync();
            return Ok(await _context.Books.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBook(Book book)
        {
            var dbBook = await _context.Books.FindAsync(book.Id);
            if (dbBook is null)
                return NotFound("Book not found.");
            dbBook.Name = book.Name;
            dbBook.Author = book.Author;

            await _context.SaveChangesAsync();
            return Ok(await _context.Books.ToListAsync());
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var dbBook = await _context.Books.FindAsync(id);
            if (dbBook is null)
                return NotFound("Book not found.");
            _context.Remove(dbBook);
            await _context.SaveChangesAsync();
            return Ok(await _context.Books.ToListAsync());
        }
    }
}
