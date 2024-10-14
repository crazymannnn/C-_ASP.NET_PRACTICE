using DotNect_SQL.Data;
using DotNect_SQL.Entities;
using DotNect_SQL.WebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNect_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly DataContext _context;

        public BorrowController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateABorrow(CreateBorrowRequest request) {
            var newBorrow = new BorrowHistory
            {
                BookId = request.BookId,
                PeopleId = request.PeopleId,
            };
            _context.BorrowHistories.Add(newBorrow);
            await _context.SaveChangesAsync();
            return Ok(newBorrow);
        }
    }
}
