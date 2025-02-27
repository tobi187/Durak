using DurakApi.Db;
using DurakApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DurakApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ApplicationDbContext context) : ControllerBase
{
    readonly ApplicationDbContext _context = context;

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Profile>> Get(Guid id)
    {
        var result = await _context.Profiles.FindAsync(id);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    // POST api/<UserController>
    [HttpGet("Create")]
    public async Task<ActionResult<Profile>> Create(string? userName)
    {
        var user = new Profile { 
            Id = Guid.NewGuid(), 
            Username = userName ?? Guid.NewGuid().ToString(),
        };
        _context.Profiles.Add(user);
        var res = await _context.SaveChangesAsync();
        if (res < 1)
            return BadRequest();
        return Ok(user);
    }

    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    public async Task Delete(Guid id)
    {
        var usr = new Profile { Id = id };
        _context.Profiles.Attach(usr);
        _context.Profiles.Remove(usr);
        await _context.SaveChangesAsync();
    }
}
