namespace DurakApi.Models.Game
{
    public class GameState
    {
        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }
        public int turnPlayer = 0;
        static readonly int handCardAmount = 6;

        public GameState(IEnumerable<Player> players) {
            Players = players.ToList();
        }

        public void StartGame()
        {
            var rnd = new Random();
            Deck = Deck.NewDeck();
            foreach (var player in Players)
            {
                var hand = Deck.GetCards(handCardAmount);
                player.HandCards.AddRange(hand);
            }
            turnPlayer = rnd.Next(0, Players.Count);

        }
    }
}
