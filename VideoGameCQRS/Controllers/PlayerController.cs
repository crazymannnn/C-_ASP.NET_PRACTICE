using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using VideoGameCQRS.Data;
using VideoGameCQRS.Models;

namespace VideoGameCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly VideoGameAppDbContext _context;

        public PlayerController(VideoGameAppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePlayer(Player player) {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return Ok(player.Id);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var player = await _context.Players.FindAsync(id);
            return Ok(player);
        }
    }
}
