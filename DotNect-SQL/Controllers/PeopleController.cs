using DotNect_SQL.Data;
using DotNect_SQL.Entities;
using DotNect_SQL.WebModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNect_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly DataContext _context;

        public PeopleController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPeople()
        {
            var people = await _context.People.Include(person => person.BorrowBooks).
                ThenInclude(book => book.Book).ToListAsync();
            var result = people.Select(people => new GetAllPeopleResponse { 
                Id = people.Id,
                Name = people.Name,
                Age = people.Age,
                Books = people.BorrowBooks.Select(book => new AuthorResponse { 
                    BookID = book.BookId,
                    BookName = book.Book.Name
                }).ToList()
            }).ToList();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPeople([FromBody] CreatePeopleRequest a)
        {
            _context.People.Add(new People
            {
                Id = a.Id,
                Name = a.Name,
                Age = a.Age,
            });
            await _context.SaveChangesAsync();
            return Ok(await _context.People.ToListAsync());
        }
    }
}
