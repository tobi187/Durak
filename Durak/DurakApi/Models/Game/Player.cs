﻿namespace DurakApi.Models.Game;

public class Player(string connectionId)
{
    public readonly string ConnectionId = connectionId;
    public string? Username { get; set; }
    public string GameId { get; set; } = Guid.NewGuid().ToString();
    public int? Place { get; set; }
    public bool IsFinished => Place != null;
    public List<Card> HandCards = [];

    public void RenewGameID() {
        Place = null;
        GameId = Guid.NewGuid().ToString();
    }

    public PlayerT ToPlayerT() => new(GameId, Username ?? "", HandCards.Count, Place);


    public void RemoveCard(Card c) {
        HandCards.Remove(c);
    }

    public MeT GetMeT()
        => new(
            ToPlayerT(),
            HandCards
        );

    public void DrawCards(Deck deck) {
        if (HandCards.Count >= 6)
            return;
        var cardsToDraw = 6 - HandCards.Count;
        HandCards.AddRange(deck.GetCards(cardsToDraw));
    }

    public bool HasPlayableCards(List<BoardCard> board) {
        foreach (var card in board)
            if (HandCards.Any(x => x.Value == card.Card.Value
                || x.Value == card.Beaten?.Card.Value))
                return true;
        return false;
    }
}

public record StateTransportT(BoardT Board, PlayersT Players);

public record BoardT(bool Locked, bool TakeRequested, int DeckCount, Card Trumpf, IEnumerable<PlayerCardT> Cards);
public record PlayerCardBeatT(Card Card, string From);
public record PlayerCardT(Card Card, string From, PlayerCardBeatT? Beaten);

public record PlayerT(string Id, string UserName, int HandAmount, int? Place);
public record PlayersT(PlayerT TurnPlayer, IEnumerable<PlayerT> Players);

public record MeT(PlayerT Info, IEnumerable<Card> Hand);

public record BeatCardR(Card CardToPlay, Card CardToBeat);

public record PlayCardR(Card CardToPlay);
