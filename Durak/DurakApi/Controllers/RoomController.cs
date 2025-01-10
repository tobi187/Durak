using DurakApi.Db;
using DurakApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DurakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController(ApplicationDbContext context) : ControllerBase
    {
        readonly ApplicationDbContext _context = context;
        
        [HttpGet]
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            var result = await _context.Rooms.ToListAsync();
            return result;
        }
    }
}
