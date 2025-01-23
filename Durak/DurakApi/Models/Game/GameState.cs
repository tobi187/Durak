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
        bool IsBoardFull => boardState.Count >= handCardAmount;
        Player GetTurnPlayer => Players[turnPlayer];

        public GameState(IEnumerable<Player> players) {
            Players = players.ToList();
        }

        public StateTransportT StartGame()
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
            return BoardStateChanged();
        }

        void NextPlayer()
        {
            if (++turnPlayer >= Players.Count)
                turnPlayer = 0;
        }

        bool TryGetPlayerAndValidate(Card card, string connId, out Player? player)
        {
            player = Players.FirstOrDefault(x => x.ConnectionId == connId);
            if (player == null)
                return false;
            if (!player.HandCards.Contains(card))
                return false;
            return true;
        }

        public StateTransportT? CanSchlag(Card card, Card cardToBeat, string connId)
        {
            if (!TryGetPlayerAndValidate(card, connId, out var player))
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
            if (!TryGetPlayerAndValidate(card, connId, out var player))
                return null;
            if (GetTurnPlayer != player)
                return null;
            if (IsBoardLocked)
                return null;
            if (!boardState.All(x => x.Card.Value == card.Value))
                return null;
            boardState.Add(new(card, player.GameId));
            NextPlayer();
            return BoardStateChanged();
        }

        bool IsAround(int num)
        {
            var higher = (num + 1) % Players.Count; 
            var lower = (num - 1) % Players.Count; 
            return higher == turnPlayer || lower == turnPlayer; 
        }

        public StateTransportT? AddCard(Card card, string connId)
        {
            var playerIndex = Players.FindIndex(x => x.ConnectionId == connId);
            if (playerIndex == -1)
                return null;
            if (IsBoardFull)
                return null;
            if (!IsAround(playerIndex))
                return null;
            if (!Players[playerIndex].HandCards.Contains(card))
                return null;
            if (!boardState.Any(x => x.Card.Value == card.Value))
                return null;
            boardState.Add(new(card, Players[playerIndex].GameId));
            return BoardStateChanged();
        }

        public StateTransportT? TakeCards(string connId)
        {
            var player = Players.FirstOrDefault(x => x.ConnectionId == connId);
            if (player == null)
                return null;
            var nCards = boardState.Select(x => x.Card);
            player.HandCards.AddRange(nCards);
            NextPlayer();
            boardState.Clear();
            return BoardStateChanged();
        }

        StateTransportT BoardStateChanged()
        {
            return new StateTransportT(
                new BoardT(
                    IsBoardLocked,
                    Deck.GetCount,
                    Deck.TrumpfCard,
                    boardState.Select(x => x.ToPlayerCardT())
                ),
                new PlayersT(
                    GetTurnPlayer.ToPlayerT(),
                    Players.Select(x => x.ToPlayerT())
                )
            );
        }
    }
}
