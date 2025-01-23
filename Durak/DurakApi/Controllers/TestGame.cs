using DurakApi.Models.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DurakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestGame : ControllerBase
    {
        [HttpGet]
        public StateTransportT GetRandomGame()
        {
            var l = new List<Player>();
            for (var i = 0; i < 4; i++)
            {
                var pl = new Player($"Player-{i}");
                pl.Username = $"Player-{i}";
            }
            var game = new GameState(l);
            var st = game.StartGame();

            return st;
        }

        public StateTransportT GetRandomGameState()
        {
            var l = new List<Player>();
            for (var i = 0; i < 4; i++)
            {
                var pl = new Player($"Player-{i}");
                pl.Username = $"Player-{i}";
            }
            var game = new GameState(l);
            var st = game.StartGame();
            game.AddCard(game.Players[game.turnPlayer].HandCards[0], l[0].GameId);

            return st;
        }

        public MeT GetRandomPlayerHand()
        {
            var player = new Player("Player-me");
            player.Username = "Player-me";
            var deck = Deck.NewDeck();
            player.DrawCards(deck);

            return player.GetMeT();
        }
    }
}
