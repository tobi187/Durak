namespace DurakApi.Models.Game;

public class GameState
{
    public Deck Deck { get; set; }
    public List<Player> Players { get; set; }
    public int turnPlayer = 0;
    static readonly int handCardAmount = 6;
    readonly List<BoardCard> boardState = [];
    bool TakeRequested = false;
    List<string> EndRequested = [];
    bool IsBoardLocked => boardState.Any(x => x.IsBeaten);
    bool IsBoardFull => boardState.Count >= handCardAmount;
    Player GetTurnPlayer => Players[turnPlayer];

    public GameState(IEnumerable<Player> players) {
        Players = [.. players];
    }

    public GameState(string connId, string? userName) {
        var player = new Player(connId);
        player.Username = userName;
        Players = [player];
    }

    public StateTransportT StartGame()
    {
        var rnd = new Random();
        Deck = Deck.NewDeck();
        boardState.Clear();
        foreach (var player in Players)
        {
            player.DrawCards(Deck);
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
        if (TakeRequested)
            return null;
        var beatenMaybe = boardState.FirstOrDefault(x => x.IsMe(cardToBeat));
        if (beatenMaybe == null)
            return null;
        var pc = new PlayerCardBeatT(card, player.GameId);
        var beaten = beatenMaybe.TrySchlag(pc, Deck.TrumpfCard.Sign);
        if (!beaten)
            return null;
        player.RemoveCard(card);
        return BoardStateChanged();
    }

    public StateTransportT? CanSchieb(Card card, string connId)
    {
        if (!TryGetPlayerAndValidate(card, connId, out var player))
            return null;
        if (GetTurnPlayer != player)
            return null;
        if (IsBoardLocked || TakeRequested)
            return null;
        if (!boardState.All(x => x.Card.Value == card.Value))
            return null;
        boardState.Add(new(card, player.GameId));
        NextPlayer();
        player.RemoveCard(card);
        return BoardStateChanged();
    }

    bool IsAround(int? num = null)
    {
        num ??= turnPlayer;
        var higher = num + 1 == Players.Count ? 0 : num + 1; 
        var lower = num == 0 ? Players.Count - 1 : num - 1;
        return higher == turnPlayer || lower == turnPlayer; //|| turnPlayer == num; 
    }

    bool IsOnBoard(Card card)
    {
        if (boardState.Count == 0)
            return false;
        if (boardState.Any(x => x.Card.Value == card.Value))
            return true;
        if (boardState.Any(x => x.Beaten?.Card.Value == card.Value))
            return true;
        return false;
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
        if (!IsOnBoard(card))
            return null;
        boardState.Add(new(card, Players[playerIndex].GameId));
        Players[playerIndex].RemoveCard(card);
        return BoardStateChanged();
    }

    public StateTransportT? OnEndRequest(string connId)
    {
        var playerIndex = Players.FindIndex(x => x.ConnectionId == connId);
        if (playerIndex == -1)
            return null;
        if (!IsAround(playerIndex))
            return null;
        if (EndRequested.Contains(connId))
            return null;
        EndRequested.Add(connId);
        if (EndRequested.Count < Math.Min(1, Players.Count - 1))
            return null;
        if (TakeRequested)
            return TakeCards(GetTurnPlayer.ConnectionId);
        if (!boardState.All(x => x.IsBeaten))
            return null;
        boardState.Clear();
        DrawCards();
        return BoardStateChanged();
    }

    public bool? RequestTakeCards(string connId)
    {
        var player = Players.FirstOrDefault(x => x.ConnectionId == connId);
        if (player == null)
            return null;
        if (player != GetTurnPlayer || boardState.Count == 0)
            return null;
        TakeRequested = true;
        if (IsBoardFull)
            return true;
        Player[] aroundPlayers = [
            Players[turnPlayer == 0 ? Players.Count - 1 : turnPlayer - 1],
            Players[turnPlayer == Players.Count - 1 ? 0 : turnPlayer + 1]];
        var actionPlayers = aroundPlayers.Where(x => x.HasPlayableCards(boardState));
        return actionPlayers.Any();
    }

    public StateTransportT? TakeCards(string connId)
    {
        if (!TakeRequested)
            return null;
        var player = Players.FirstOrDefault(x => x.ConnectionId == connId);
        if (player == null || player != GetTurnPlayer)
            return null;
        TakeRequested = false;
        var nCards = boardState.Select(x => x.Card);
        player.HandCards.AddRange(nCards);
        NextPlayer();
        boardState.Clear();
        DrawCards();
        return BoardStateChanged();
    }

    void DrawCards()
    {
        // TODO: change draw order; remember starting player
        for (int i = turnPlayer + 1; i < Players.Count; i++)
            Players[i].DrawCards(Deck);
        for (int i = 0; i <= turnPlayer; i++)
            Players[i].DrawCards(Deck);
    }

    StateTransportT BoardStateChanged()
    {
        EndRequested = []; // TODO: check if this is ok ?
        return new StateTransportT(
            new BoardT(
                IsBoardLocked,
                TakeRequested,
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

    public void RemovePlayer(string connectionId)
    {
        return;
    }

    public IEnumerable<PlayerT> AddPlayer(string connId, string? userName)
    {
        var player = new Player(connId);
        player.Username = userName;
        Players.Add(player);
        return Players.Select(x => x.ToPlayerT());
    }
}
