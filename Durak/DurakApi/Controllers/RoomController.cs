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
        
        [HttpGet("GetAll")]
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            var result = await _context.Rooms.ToListAsync();
            return result;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> CreateRoom(User? user, string? roomName)
        {
            var room = new Room();
            room.Users = [user];
            room.Name = roomName ?? Guid.NewGuid().ToString();
            _context.Rooms.Add(room);
            var rowsChanged = await _context.SaveChangesAsync();
            if (rowsChanged == 0)
                return BadRequest();
            return Ok(room.Id);
        }
    }
}
