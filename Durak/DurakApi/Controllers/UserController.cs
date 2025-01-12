using DurakApi.Db;
using DurakApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DurakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ApplicationDbContext context) : ControllerBase
    {
        readonly ApplicationDbContext _context = context;

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(Guid id)
        {
            var result = await _context.Users.FindAsync(id);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        // POST api/<UserController>
        [HttpGet("Create")]
        public async Task<ActionResult<User>> Create(string? userName)
        {
            var user = new User { 
                Id = Guid.NewGuid(), 
                Username = userName ?? Guid.NewGuid().ToString(),
            };
            _context.Users.Add(user);
            var res = await _context.SaveChangesAsync();
            if (res < 1)
                return BadRequest();
            return Ok(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            var usr = new User { Id = id };
            _context.Users.Attach(usr);
            _context.Users.Remove(usr);
            await _context.SaveChangesAsync();
        }
    }
}
