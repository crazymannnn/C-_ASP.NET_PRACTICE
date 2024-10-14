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
    //duytest
    public class AuthorController : ControllerBase
    {
        private readonly DataContext _context;

        public AuthorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthor() {
            var authors = await _context.Authors.Include(author => author.Books).ToListAsync();
            var result = authors.Select(author => new GetAllAuthorResponse
            {
                Id = author.Id,
                Name = author.Name,
                Age = author.Age,
                Books = author.Books.Select(book => new AuthorResponse
                {
                    BookID = book.Id,
                    BookName = book.Name,
                })
                .ToList()
            }).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var a = await _context.Authors.FindAsync(id);
            if (a is null) 
                return NotFound();
            return Ok(a);
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody]CreateAuthorRequest a) { 
            _context.Authors.Add(new Author {
                Id = a.Id,
                Name = a.Name,
                Age = a.Age,
            });
            await _context.SaveChangesAsync();
            return Ok(await _context.Authors.ToListAsync());
        }

        [HttpPut]
        public async Task<IActionResult> Update(Author a) {
            var currentAuthor = await _context.Authors.FindAsync(a.Id);
            if (currentAuthor is null) 
                return NotFound();
            currentAuthor.Name = a.Name;
            currentAuthor.Age = a.Age;
            await _context.SaveChangesAsync();
            return Ok(await _context.Authors.ToListAsync());
        }

        [HttpPut("{update_no_track}")]
        public async Task<IActionResult> UpdateNoTrack(Author a)
        {
            var currentAuthor = _context.Authors.AsNoTracking().FirstOrDefault(s => s.Id == a.Id);
            if (currentAuthor is null)
                return NotFound();
            currentAuthor.Name = a.Name;
            currentAuthor.Age = a.Age;
            //await _context.SaveChangesAsync();
            return Ok(a.Id+" "+a.Name +" "+a.Age);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var currentA = await _context.Authors.FindAsync(id);
            if (currentA is null) { 
                return NotFound();
            }
            _context.Remove(currentA);
            await _context.SaveChangesAsync();
            return Ok(await _context.Authors.ToListAsync());
        }
    }
}
