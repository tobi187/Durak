namespace DurakApi.Models.Game
{
    public class GameState
    {
        public Deck Deck { get; set; }
        public Player[] Players { get; set; }
        public int turnPlayer = 0;

        public GameState(IEnumerable<Player> players) {
            Deck = Deck.NewDeck();
            Players = players.ToArray();
        }


    }
}
