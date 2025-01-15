using DurakApi.Db;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DurakApi.Hubs
{
    public class DurakHub : Hub
    {
        public async Task CreateRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Group(roomId).SendAsync("UserJoined", "ka");
        }

        public async Task StartGame(string roomId, ApplicationDbContext context)
        {
            var rId = Guid.Parse(roomId);
            var room = await context.Rooms.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == rId);
            if (room == null || !room.CanPlay || room.IsPlaying)
                return;
            room.IsPlaying = true;
            context.Update(room);
            await context.SaveChangesAsync();
            await Clients.Group(roomId).SendAsync("StartGame", "");
        }
    }
}
