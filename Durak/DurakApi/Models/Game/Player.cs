namespace DurakApi.Models.Game
{
    public class Player(string connectionId)
    {
        public readonly string ConnectionId = connectionId;

        public List<Card> HandCards = [];


    }
}
