namespace DurakApi.Models.Game
{
    public class Player(string connectionId)
    {
        public readonly string ConnectionId = connectionId;
        public string? Username { get; set; }
        public string GameId { get; private set; }

        public List<Card> HandCards = [];
        public void RenewGameID()
        {
            GameId = Guid.NewGuid().ToString();
        }

        public PlayerT ToPlayerT()
        {
            return new PlayerT(GameId, Username ?? "");
        }
    }

    public record StateTransportT(BoardT Board, PlayersT Players);

    public record BoardT(bool Locked, IEnumerable<PlayerCardT> Cards);
    public record PlayerCardBeatT(Card Card, string From);
    public record PlayerCardT(Card Card, string From, PlayerCardBeatT? Beaten);

    public record PlayerT(string Id, string UserName);
    public record PlayersT(PlayerT TurnPlayer, IEnumerable<PlayerT> Players);

    public record MeT(PlayerT Info, IEnumerable<Card> Hand);

    public record PlayCardR(Card CardToPlay, Card CardToBeat);

    public record SchiebCardR(Card CardToPlay);
}
