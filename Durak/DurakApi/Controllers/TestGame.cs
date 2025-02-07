using DurakApi.Models.Game;
using Microsoft.AspNetCore.Mvc;

namespace DurakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestGame : ControllerBase
    {
        [HttpGet("GetRandomGame")]
        public StateTransportT GetRandomGame()
        {
            var l = new List<Player>();
            for (var i = 0; i < 4; i++)
            {
                var pl = new Player($"Player-{i}");
                pl.Username = $"Player-{i}";
                pl.GameId = $"Player-{i}";
                l.Add(pl);
            }
            var game = new GameState(l);
            var st = game.StartGame();

            return st;
        }

        [HttpGet("GetRandomGameState")]
        public StateTransportT? GetRandomGameState()
        {
            var l = new List<Player>();
            for (var i = 0; i < 4; i++)
            {
                var pl = new Player($"Player-{i}");
                pl.Username = $"Player-{i}";
                pl.GameId = $"Player-{i}";
                l.Add(pl);
            }
            var game = new GameState(l);
            game.StartGame();
            var c = game.Players[game.turnPlayer].HandCards.Where(x => x.Sign != game.Deck.TrumpfCard.Sign).MinBy(x => x.Value);
            var ac = game.AddCard(c, game.Players[game.turnPlayer].ConnectionId);
            var cc = game.Players[game.turnPlayer].HandCards.Where(
                x => x.Sign == game.Deck.TrumpfCard.Sign
                || (x.Sign == c.Sign && x.Value > c.Value)).First();

            //var schlag = game.CanSchlag(cc, c, game.Players[game.turnPlayer].ConnectionId);
            return ac;
        }

        [HttpGet("GetRandomPlayerHand")]
        public MeT GetRandomPlayerHand()
        {
            var player = new Player("Player-me");
            player.GameId = "Player-me";
            player.Username = "Player-me";
            var deck = Deck.NewDeck();
            player.DrawCards(deck);

            return player.GetMeT();
        }
    }
}
