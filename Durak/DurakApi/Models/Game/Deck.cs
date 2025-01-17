﻿namespace DurakApi.Models.Game
{
    public enum CardSign
    {
        Herz = 1,
        Shippe = 2,
        Karo = 3,
        Kreuz = 4
    }

    public enum CardValue
    {
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Bube = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }

    public record Card(CardSign Sign, CardValue Value);

    public class Deck(List<Card> cards, Card trumpf)
    {
        readonly List<Card> Cards = cards;
        public readonly Card TrumpfCard = trumpf;
        public int GetCount => Cards.Count;

        public static Deck NewDeck()
        {
            var cardList = new List<Card>();
            foreach (var sign in Enum.GetValues<CardSign>())
                foreach (var val in Enum.GetValues<CardValue>())
                    cardList.Add(new Card(sign, val));

            var rnd = new Random();
            cardList = [.. cardList.OrderBy(_ => rnd.Next())];
            return new Deck(cardList, cardList.Last());
        }

        public List<Card> GetCards(int amount)
        {
            // TODO: maybe better Data Structure
            var end = Math.Min(Cards.Count, amount);
            var cards = Cards[0..end];
            Cards.RemoveRange(0, end);
            return cards;
        }
    }
}
