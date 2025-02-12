using DurakApi.Db;
using DurakApi.Models;
using DurakApi.Models.Game;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Concurrent;

namespace DurakApi.Hubs
{
    public class DurakHub : Hub
    {
        static readonly ConcurrentDictionary<string, GameState> games = new();
        static readonly ConcurrentDictionary<string, string> userRooms = new();

        public async Task CreateRoom(HubUserModel model)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, model.roomId);
            games[model.roomId] = new GameState(Context.ConnectionId, model.userName);
            userRooms[Context.ConnectionId] = model.roomId;
        }

        public async Task JoinRoom(HubUserModel model)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, model.roomId);
            if (!games.TryGetValue(model.roomId, out var gameState))
                return;
            var players = gameState.AddPlayer(Context.ConnectionId, model.userName);
            userRooms[Context.ConnectionId] = model.roomId;
            await Clients.Group(model.roomId).SendAsync("UserJoined", players);
        }

        public async Task StartGame(HubBaseModel model, ApplicationDbContext context)
        {
            var rId = Guid.Parse(model.roomId);
            if (!games.TryGetValue(model.roomId, out var gameState))
                return;
            var room = await context.Rooms.Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == rId);
            if (room == null || !room.CanPlay || room.IsPlaying)
                return;
            room.IsPlaying = true;
            context.Update(room);
            await context.SaveChangesAsync();
            var state = gameState!.StartGame();
            await Clients.Group(model.roomId).SendAsync("StartGame", state);
            Log.Information("GameState: {@state}", state);
            await SendHands(gameState);
        }

        
        async Task SendHands(GameState state)
        {
            foreach (var player in state.Players)
            {
                var cards = player.GetMeT();
                Log.Information("PLayerkarten: {@cards}", cards);
                await Clients.Client(player.ConnectionId).SendAsync("Hand", cards);
            }
        }

        async Task SendCallerHand(GameState state)
        {
            var player = state.Players.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (player == null) return;
            var cards = player.GetMeT();
            Log.Information("Send New Hand Player {@cards}", cards);
            await Clients.Caller.SendAsync("Hand", cards);
        }

        public async Task AddCard(HubCardModel model)
        {
            Log.Information("Rec HubCardModel {@model}", model);
            var game = games[model.roomId];
            if (game == null)
                return;
            StateTransportT? state;
            lock (model.roomId) {
                state = game.AddCard(model.card, Context.ConnectionId);
            }
            if (state == null)
                return;
            Log.Information("Send state: {@state}", state);
            await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
            await SendCallerHand(game);
        }

        public async Task SchlagCard(HubCardToBeatModel model)
        {
            Log.Information("Rec HubCardToBeatModel {@model}", model);
            var game = games[model.roomId];
            if (game == null) return;
            StateTransportT? state;
            lock (model.roomId)
            {
                state = game.CanSchlag(model.card, model.cardToBeat, Context.ConnectionId);
            }
            if (state == null) return;
            Log.Information("Send State {@state}", state);
            await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
            await SendCallerHand(game);
        }

        public async Task SchiebCard(HubCardModel model)
        {
            Log.Information("Rec HubCardModel {@model}", model);
            var game = games[model.roomId];
            if (game == null) return;
            StateTransportT? state;
            lock (model.roomId)
            {
                state = game.CanSchieb(model.card, Context.ConnectionId);
            }
            if (state == null) return;
            Log.Information("Send State {@state}", state);
            await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
            await SendCallerHand(game);
        }

        public async Task TakeCards(HubBaseModel model)
        {
            Log.Information("Rec HubBseModel {@model}", model);
            var game = games[model.roomId];
            if (game == null) return;
            StateTransportT? state;
            lock (model.roomId)
            {
                state = game.TakeCards(Context.ConnectionId);
            }
            if (state == null) return;
            Log.Information("Send State {@state}", state);
            await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
            await SendHands(game);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (exception != null)
                Log.Information(exception, "Player disconnected from Hub");

            if (!userRooms.TryGetValue(Context.ConnectionId, out var room))
                return base.OnDisconnectedAsync(exception);

            var game = games[room];
            game.RemovePlayer(Context.ConnectionId);

            // TODO: DB Stuff

            return base.OnDisconnectedAsync(exception);
        }
    }
}
