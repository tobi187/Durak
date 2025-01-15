using DurakApi.Db;
using DurakApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DurakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController(ApplicationDbContext context) : ControllerBase
    {
        readonly ApplicationDbContext _context = context;
        static readonly int MaxPlayerCount = 4;
        
        [HttpGet("GetAll")]
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            var result = await _context.Rooms
                .Where(x => x.IsPlaying == false && x.Users.Count < MaxPlayerCount)
                .Include(x => x.Users).ToListAsync();
            return result;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> CreateRoom(User user, string? roomName)
        {
            var room = new Room();
            room.Users = [user];
            room.Name = roomName ?? Guid.NewGuid().ToString();
            _context.Users.Attach(user);
            await _context.Rooms.AddAsync(room);
            var rowsChanged = await _context.SaveChangesAsync();
            if (rowsChanged == 0)
                return BadRequest();
            return Ok(room.Id);
        }

        [HttpPost("Join")]
        public async Task<ActionResult<Guid>> Join(Guid roomId, User user)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return BadRequest("Room not found");
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
}
