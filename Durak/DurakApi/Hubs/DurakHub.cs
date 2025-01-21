using DurakApi.Db;
using DurakApi.Models.Game;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace DurakApi.Hubs
{
    public class DurakHub : Hub
    {
        static ConcurrentDictionary<string, GameState> games = new();

        public async Task CreateRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            games[roomId] = new GameState([new Player(Context.ConnectionId)]);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            if (!games.TryGetValue(roomId, out GameState gameState))
                return;
            gameState.Players.Add(new Player(Context.ConnectionId));
            await Clients.Group(roomId).SendAsync("UserJoined", "ka");
        }

        public async Task StartGame(string roomId, ApplicationDbContext context)
        {
            var rId = Guid.Parse(roomId);
            if (games.TryGetValue(roomId, out GameState gameState))
                return;
            var room = await context.Rooms.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == rId);
            if (room == null || !room.CanPlay || room.IsPlaying)
                return;
            room.IsPlaying = true;
            context.Update(room);
            await context.SaveChangesAsync();
            gameState.StartGame();
            await Clients.Group(roomId).SendAsync("StartGame", "");
        }
    }
}
