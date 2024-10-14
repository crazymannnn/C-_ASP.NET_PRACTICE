using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entity.Entities;

namespace Entity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllHeros() {
            var heroes = new List<SuperHero> {
                new SuperHero { 
                    Id = 1,
                    Name = "Test1",
                    FirstName = "Test2",
                    LastName = "Test3",
                    Place = "Test4"
                }
            };
            return Ok(heroes);
        }
    }
}
