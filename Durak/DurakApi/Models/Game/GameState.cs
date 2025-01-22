namespace DurakApi.Models.Game
{
    public class GameState
    {
        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }
        public int turnPlayer = 0;
        static readonly int handCardAmount = 6;
        readonly List<BoardCard> boardState = [];
        bool IsBoardLocked => boardState.Any(x => x.IsBeaten);
        Player GetTurnPlayer => Players[turnPlayer];

        public GameState(IEnumerable<Player> players) {
            Players = players.ToList();
        }

        public void StartGame()
        {
            var rnd = new Random();
            Deck = Deck.NewDeck();
            boardState.Clear();
            foreach (var player in Players)
            {
                var hand = Deck.GetCards(handCardAmount);
                player.HandCards.AddRange(hand);
                player.RenewGameID();
            }
            turnPlayer = rnd.Next(0, Players.Count);
        }

        public StateTransportT? CreateSharedState()
        {
            return new StateTransportT(
                    new BoardT(false, []),
                    new PlayersT(
                        Players[turnPlayer].ToPlayerT(), 
                        Players.Select(x => x.ToPlayerT())
                    )
                );
        }

        void NextPlayer()
        {
            if (++turnPlayer >= Players.Count)
                turnPlayer = 0;
        }

        public StateTransportT? CanPlay(Card card, Card cardToBeat, string connId)
        {
            var player = Players.FirstOrDefault(x => x.ConnectionId == connId);
            if (player == null)
                return null;
            if (GetTurnPlayer != player)
                return null;
            var beatenMaybe = boardState.FirstOrDefault(x => x.IsMe(cardToBeat));
            if (beatenMaybe == null)
                return null;
            var pc = new PlayerCardBeatT(card, player.GameId);
            var beaten = beatenMaybe.TrySchlag(pc, Deck.TrumpfCard.Sign);
            if (!beaten)
                return null;
            return BoardStateChanged();
        }

        public StateTransportT? CanSchieb(Card card, string connId)
        {
            var player = Players.FirstOrDefault(x => x.ConnectionId == connId);
            if (player == null)
                return null;
            if (GetTurnPlayer != player)
                return null;
            if (IsBoardLocked)
                return null;
        }

        StateTransportT BoardStateChanged()
        {

        }
    }
}
