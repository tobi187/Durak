using DurakApi.Db;
using DurakApi.Helpers;
using DurakApi.Models.Db;
using DurakApi.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DurakApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RoomController(ApplicationDbContext context) : ControllerBase
{
    readonly ApplicationDbContext _context = context;
    static readonly int MaxPlayerCount = 4;
    
    [HttpGet("GetAll")]
    public async Task<IEnumerable<RoomInfoModelS>> GetAllRooms()
    {
        var result = await _context.Rooms
            .Include(x => x.Users)
            .Where(x => x.Users.Count > 0 && !x.IsPlaying)
            .ToListAsync();
        
        return result.Select(x => x.ToRoomInfo());
    }

    [HttpGet("GetRoom")]
    public async Task<IResult> GetRoomInfo(Guid roomId)
    {
        var result = await _context.Rooms
            .Include(x => x.Rules)
            .FirstOrDefaultAsync(x => x.Id == roomId);

        if (result is null)
            return TypedResults.BadRequest();
        return TypedResults.Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IResult> CreateRoom(RoomNameModelR model)
    {
        var user = await _context.Profiles.FindAsync(AuthHelper.FindId(User));
        if (user == null)
            return TypedResults.Forbid();
        var room = Room.New(user, model.RoomName);
        _context.Profiles.Attach(user);
        await _context.Rooms.AddAsync(room);
        var rowsChanged = await _context.SaveChangesAsync();
        if (rowsChanged == 0)
            return TypedResults.BadRequest();
        return TypedResults.Ok(room.Id);
    }

    [HttpPost("Join")]
    public async Task<ActionResult<Guid>> Join(RoomIdModelR model)
    {
        var user = await _context.Profiles.FindAsync(AuthHelper.FindId(User));
        var room = await _context.Rooms.Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == model.RoomId);
        if (room == null || user == null)
            return BadRequest("Not found");
        if (room.IsPlaying || room.Users.Count >= MaxPlayerCount)
            return BadRequest("Room is full");
        room.Users.Add(user);
        _context.Rooms.Update(room);
        var res = await _context.SaveChangesAsync();
        if (res < 1)
            return BadRequest();
        return Ok(room.Id);
    }
}
