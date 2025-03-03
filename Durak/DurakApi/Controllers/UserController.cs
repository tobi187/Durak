using DurakApi.Db;
using DurakApi.Helpers;
using DurakApi.Models.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DurakApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(ApplicationDbContext context) : ControllerBase
{
    readonly ApplicationDbContext _context = context;

    [HttpGet("Me")]
    public async Task<ActionResult<Profile>> Me()
    {
        var result = await _context.Profiles.FindAsync(AuthHelper.FindId(User));
        if (result is null)
        {
            result = Profile.New(User);
            await _context.Profiles.AddAsync(result);
            await _context.SaveChangesAsync();
        }
        return Ok(result);
    }

    public record UserNameChangeReq(string Username);

    [HttpPost("Rename")]
    public async Task<IResult> Rename(UserNameChangeReq req)
    {
        var res = await _context.Profiles.FindAsync(AuthHelper.FindId(User));
        if (res == null)
            return Results.NotFound();
        res.Username = req.Username;
        await _context.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
