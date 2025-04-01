using System.Diagnostics.CodeAnalysis;

namespace DurakApi.Models.Game;

public class GameState
{
    public Deck Deck { get; set; } = Deck.NewDeck();
    public List<Player> Players { get; set; }
    int startedTurnPlayer = 0;
    int turnPlayer = 0;
    static readonly int handCardAmount = 6;
    readonly List<BoardCard> boardState = [];
    bool TakeRequested = false;
    List<string> EndRequested = [];
    bool IsBoardLocked => boardState.Any(x => x.IsBeaten);
    bool IsBoardFull => boardState.Count >= handCardAmount;
    int ActiveCount => Players.Count(x => !x.IsFinished);
    Player GetTurnPlayer => Players[turnPlayer];

    public GameState(IEnumerable<Player> players) {
        Players = [.. players];
    }

    public GameState(string connId, string? userName) {
        var player = new Player(connId);
        player.Username = userName;
        Players = [player];
    }

    public StateTransportT StartGame() {
        var rnd = new Random();
        Deck = Deck.NewDeck();
        boardState.Clear();
        foreach (var player in Players) {
            player.DrawCards(Deck);
            player.RenewGameID();
        }
        TakeRequested = false;
        EndRequested = [];
        turnPlayer = rnd.Next(Players.Count);
        startedTurnPlayer = turnPlayer;
        return BoardStateChanged();
    }

    void NextPlayer() {
        var max = 0;
        // wenn alles geschlagen ist darf der Spieler der geschlagen hat anfangen
        if (boardState.All(x => x.IsBeaten) && !GetTurnPlayer.IsFinished) {
            return;
        }
        do {
            if (++turnPlayer >= Players.Count)
                turnPlayer = 0;
        } while (GetTurnPlayer.IsFinished && max++ <= Players.Count);
    }

    bool TryGetPlayerAndValidate(string connId, [NotNullWhen(true)] out Player? player) {
        player = Players.FirstOrDefault(x => x.ConnectionId == connId);
        if (player == null)
            return false;
        if (player.IsFinished)
            return false;
        return true;
    }

    bool TryGetPlayerAndValidate(string connId, Card card, [NotNullWhen(true)] out Player? player) {
        player = Players.FirstOrDefault(x => x.ConnectionId == connId);
        if (player == null)
            return false;
        if (player.IsFinished)
            return false;
        if (!player.HandCards.Contains(card))
            return false;
        return true;
    }

    public StateTransportT? CanSchlag(Card card, Card cardToBeat, string connId) {
        if (!TryGetPlayerAndValidate(connId, card, out var player))
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

    // use this in nextplayer
    int GetNextPlayer() {
        var max = 0;
        var curr = (turnPlayer + 1) % Players.Count;
        do {
            if (++curr >= Players.Count)
                curr = 0;
            // TODO: wenn max erreicht return was anderes vmtlch
        } while (Players[curr].IsFinished && max++ <= Players.Count);
        return curr;
    }

    // check handkarten anderer spieler 
    public StateTransportT? CanSchieb(Card card, string connId) {
        if (!TryGetPlayerAndValidate(connId, card, out var player))
            return null;
        if (GetTurnPlayer != player)
            return null;
        if (IsBoardLocked || TakeRequested)
            return null;
        if (!boardState.All(x => x.Card.Value == card.Value))
            return null;
        var next = GetNextPlayer();
        if (Players[next].HandCards.Count <= boardState.Count(x => !x.IsBeaten))
            return null;
        boardState.Add(new(card, player.GameId));
        NextPlayer();
        player.RemoveCard(card);
        return BoardStateChanged();
    }

    Player[] GetAround() {
        List<Player> around = [];
        int num = turnPlayer;

        // Search right (forward)
        while (around.Count == 0) {
            num = (num + 1) % Players.Count; // Ensure wrapping
            if (num == turnPlayer) break; // Stop if we complete a full loop

            if (!Players[num].IsFinished)
                around.Add(Players[num]);
        }

        num = turnPlayer;

        // Search left (backward)
        while (around.Count == 1) {
            num = (num - 1 + Players.Count) % Players.Count; // Ensure wrapping
            if (num == turnPlayer) break; // Stop if we complete a full loop

            if (!Players[num].IsFinished && around.First() != Players[num])
                around.Add(Players[num]);
        }

        return [.. around];
    }

    bool IsOnBoard(Card card) {
        if (boardState.Count == 0)
            return false;
        if (boardState.Any(x => x.Card.Value == card.Value))
            return true;
        if (boardState.Any(x => x.Beaten?.Card.Value == card.Value))
            return true;
        return false;
    }

    public StateTransportT? AddCard(Card card, string connId) {
        if (!TryGetPlayerAndValidate(connId, card, out var player))
            return null;
        var unbeaten = boardState.Count(x => !x.IsBeaten);
        if (IsBoardFull || GetTurnPlayer.HandCards.Count <= unbeaten)
            return null;
        var around = GetAround();
        if (!around.Contains(player))
            return null;
        if (!IsOnBoard(card))
            return null;
        boardState.Add(new(card, player.GameId));
        player.RemoveCard(card);
        return BoardStateChanged();
    }

    public StateTransportT? OnEndRequest(string connId) {
        if (!TryGetPlayerAndValidate(connId, out var player))
            return null;
        var around = GetAround();
        if (!around.Contains(player))
            return null;
        if (EndRequested.Contains(connId))
            return null;
        EndRequested.Add(connId);
        if (EndRequested.Count < Math.Min(ActiveCount - 1, 2)) // check if this ok
            return null;
        if (TakeRequested)
            return TakeCards(GetTurnPlayer.ConnectionId);
        if (!boardState.All(x => x.IsBeaten))
            return null;
        OnNextTurn();
        return BoardStateChanged();
    }

    public bool? RequestTakeCards(string connId) {
        if (!TryGetPlayerAndValidate(connId, out var player))
            return null;
        if (player != GetTurnPlayer || boardState.Count == 0)
            return null;
        TakeRequested = true;
        if (IsBoardFull)
            return true;
        var actionPlayers = GetAround().Where(x => x.HasPlayableCards(boardState));
        return actionPlayers.Any();
    }

    public StateTransportT? TakeCards(string connId) {
        if (!TakeRequested)
            return null;
        var player = Players.FirstOrDefault(x => x.ConnectionId == connId);
        if (player == null || player != GetTurnPlayer)
            return null;
        TakeRequested = false;
        foreach (var card in boardState) {
            player.HandCards.Add(card.Card);
            if (card.IsBeaten) {
                player.HandCards.Add(card.Beaten!.Card);
            }
        }
        OnNextTurn();
        return BoardStateChanged();
    }

    void OnNextTurn() {
        DrawCards();
        CheckWin();
        NextPlayer();
        startedTurnPlayer = turnPlayer;
        boardState.Clear();
    }

    void CheckWin() {
        if (!Deck.IsEmpty)
            return;
        var opts = Players.Where(x => x.Place != null
            && x.HandCards.Count == 0).ToArray();
        if (opts.Length == 0)
            return;
        var lMax = Players.Select(x => x.Place).Max();
        var boardOrder = boardState.Select(x => x.From).Distinct().ToArray();
        var orderedByWinner = opts.OrderBy(x => Array.IndexOf(boardOrder, x.GameId));
        foreach (var winner in orderedByWinner)
            if (winner != GetTurnPlayer)
                winner.Place = ++lMax;
        if (GetTurnPlayer.HandCards.Count == 0)
            GetTurnPlayer.Place = ++lMax;
        var remaining = Players.Where(x => !x.IsFinished);
        if (remaining.Count() == 1)
            remaining.First().Place = ++lMax;
    }

    void DrawCards() {
        // startplayer zieht erst, dann gehts im Kreis weiter
        // falls geschlagen wurde zieht der Spieler, welcher geschlagen hat als letztes
        var start = startedTurnPlayer == turnPlayer ? (turnPlayer + 1) % Players.Count : startedTurnPlayer;
        for (int i = start; i < Players.Count; i++)
            if (i != turnPlayer)
                Players[i].DrawCards(Deck);
        for (int i = 0; i < start; i++)
            if (i != turnPlayer)
                Players[i].DrawCards(Deck);
        
        GetTurnPlayer.DrawCards(Deck);
    }

    StateTransportT BoardStateChanged() {
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

    public StateTransportT? RemovePlayer(string connectionId) {
        // how to act on player leave ?
        // assumption: playercards get removed
        if (!TryGetPlayerAndValidate(connectionId, out var player))
            return null;
        Players.Remove(player);
        if (ActiveCount < 2) {
            // finish up
        } else if (GetTurnPlayer == player) {
            OnNextTurn();
        } else {
            boardState.RemoveAll(x => x.From == player.GameId);
        }
        return BoardStateChanged();
    }

    public IEnumerable<PlayerT> AddPlayer(string connId, string? userName) {
        var player = new Player(connId);
        player.Username = userName;
        Players.Add(player);
        return Players.Select(x => x.ToPlayerT());
    }
}
