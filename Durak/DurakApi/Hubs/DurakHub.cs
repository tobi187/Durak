using Serilog;
using DurakApi.Db;
using DurakApi.Models;
using DurakApi.Models.Game;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;

namespace DurakApi.Hubs;

[Authorize]
public class DurakHub : Hub
{
    static readonly ConcurrentDictionary<string, GameState> games = new();

    public async Task CreateRoom(HubUserModel model)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, model.roomId);
        games[model.roomId] = new GameState(Context.ConnectionId, model.userName);
    }

    public async Task JoinRoom(HubUserModel model)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, model.roomId);
        if (!games.TryGetValue(model.roomId, out var gameState))
            return;
        var players = gameState.AddPlayer(Context.ConnectionId, model.userName);
        await Clients.Group(model.roomId).SendAsync("UserJoined", players);
    }

    public async Task StartGame(HubBaseModel model, ApplicationDbContext context)
    {
        Log.Information("[StartGame] Recv HubBaseModel: {@model}", model);
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
        Log.Information("[StartGame] Send GameState: {@state}", state);
        await SendHands(gameState);
    }

    
    async Task SendHands(GameState state)
    {
        foreach (var player in state.Players)
        {
            var cards = player.GetMeT();
            Log.Information("[SendHands] Send Playerkarten: {@cards}", cards);
            await Clients.Client(player.ConnectionId).SendAsync("Hand", cards);
        }
    }

    async Task SendCallerHand(GameState state)
    {
        var player = state.Players.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        if (player == null) return;
        var cards = player.GetMeT();
        Log.Information("[SendCallerHand] Send New Hand Player {@cards}", cards);
        await Clients.Caller.SendAsync("Hand", cards);
    }

    public async Task AddCard(HubCardModel model)
    {
        Log.Information("[AddCard] Recv HubCardModel {@model}", model);
        var game = games[model.roomId];
        if (game == null)
            return;
        StateTransportT? state;
        lock (model.roomId)
            state = game.AddCard(model.card, Context.ConnectionId);
        
        if (state == null)
            return;
        Log.Information("[AddCard] Send state: {@state}", state);
        await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
        await SendCallerHand(game);
    }

    public async Task SchlagCard(HubCardToBeatModel model)
    {
        Log.Information("[SchlagCard] Recv HubCardToBeatModel {@model}", model);
        var game = games[model.roomId];
        if (game == null) return;
        StateTransportT? state;
        lock (model.roomId)
            state = game.CanSchlag(model.card, model.cardToBeat, Context.ConnectionId);
        
        if (state == null) return;
        Log.Information("[SchlagCard] Send State {@state}", state);
        await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
        await SendCallerHand(game);
    }

    public async Task SchiebCard(HubCardModel model)
    {
        Log.Information("[SchiebCard] Recv HubCardModel {@model}", model);
        var game = games[model.roomId];
        if (game == null) return;
        StateTransportT? state;
        lock (model.roomId)
            state = game.CanSchieb(model.card, Context.ConnectionId);
        
        if (state == null) return;
        Log.Information("[SchiebCard] Send State {@state}", state);
        await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
        await SendCallerHand(game);
    }

    public async Task OnEndRequested(HubBaseModel model)
    {
        Log.Information("[OnEndRequested] Recv HubBaseModel {@model}", model);
        var game = games[model.roomId];
        if (game == null) return;
        StateTransportT? state;
        lock (model.roomId)
            state = game.OnEndRequest(Context.ConnectionId);
        if (state == null)
            return;

        Log.Information("[OnEndRequested] Send End Accepted {@state}", state);
        await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
        await SendHands(game);
    }

    public async Task OnTakeCardsRequested(HubBaseModel model)
    {
        Log.Information("[OnTakeCardsRequested] Recv HubBaseModel {@model}", model);
        var game = games[model.roomId];
        if (game == null) return;
        bool? actionLeft;
        lock (model.roomId)
            actionLeft = game.RequestTakeCards(Context.ConnectionId);

        if (actionLeft == null)
            return;
        if (actionLeft.Value)
        {
            Log.Information("[OnTakeCardsRequested] Waiting for players To Add Cards (10 sec)");
            await Clients.Group(model.roomId).SendAsync("TakeRequested");
            await Task.Delay(TimeSpan.FromSeconds(10));
            Log.Information("[OnTakeCardsRequested] Finished Waiting");
        }
        StateTransportT? state;
        lock (model.roomId)
            state = game.TakeCards(Context.ConnectionId);

        if (state == null)
            return;
        
        Log.Information("[TakeCards] Send State {@state}", state);
        await Clients.Group(model.roomId).SendAsync("GameStateChanged", state);
        await SendHands(game);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Log.Information(exception, "Player disconnected from Hub");
        // TODO: DB Stuff

        return base.OnDisconnectedAsync(exception);
    }
}
