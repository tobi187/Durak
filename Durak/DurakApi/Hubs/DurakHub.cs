using DurakApi.Db;
using DurakApi.Models.Game;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace DurakApi.Hubs
{
    public class DurakHub : Hub
    {
        static readonly ConcurrentDictionary<string, GameState> games = new();

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
            if (games.TryGetValue(roomId, out var gameState))
                return;
            var room = await context.Rooms.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == rId);
            if (room == null || !room.CanPlay || room.IsPlaying)
                return;
            room.IsPlaying = true;
            context.Update(room);
            await context.SaveChangesAsync();
            var state = gameState!.StartGame();
            await Clients.Group(roomId).SendAsync("StartGame", state);
            await SendHands(gameState);
        }

        async Task SendHands(GameState state)
        {
            foreach (var player in state.Players)
            {
                var cards = player.GetMeT();
                await Clients.Client(player.ConnectionId).SendAsync("Hand", cards);
            }
        }

        public async Task AddCard(string roomId, Card card)
        {
            var game = games[roomId];
            if (game == null)
                return;
            var state = game.AddCard(card, Context.ConnectionId);
            if (state == null)
                return;
            await Clients.Group(roomId).SendAsync("GameStateChanged", state);
        }

        public async Task SchlagCard(string roomId, Card card, Card cardToBeat)
        {
            var game = games[roomId];
            if (game == null) return;
            var state = game.CanSchlag(card, cardToBeat, Context.ConnectionId);
            if (state == null) return;
            await Clients.Group(roomId).SendAsync("GameStateChanged", state);
        }

        public async Task SchiebCard(string roomId, Card card)
        {
            var game = games[roomId];
            if (game == null) return;
            var state = game.CanSchieb(card, Context.ConnectionId);
            if (state == null) return;
            await Clients.Group(roomId).SendAsync("GameStateChanged", state);
        }

        public async Task TakeCards(string roomId)
        {
            var game = games[roomId];
            if (game == null) return;
            var  state = game.TakeCards(Context.ConnectionId);
            if (state == null) return;
            await Clients.Group(roomId).SendAsync("GameStateChanged", state);
            await SendHands(game);
        }
    }
}
