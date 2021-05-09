using System;
using DecksSorter.Models;

namespace DecksSorter.Shufflers
{
    public class SimpleShuffler : IShuffler
    {
        private readonly Random rnd = new();
        public void Shuffle(Deck deck)
        {
            for (var i = deck.Cards.Count - 1; i >= 1; i--)
            {
                var j = rnd.Next(i + 1);
                var temp = deck.Cards[j].PositionInDeck;
                deck.Cards[j].PositionInDeck = deck.Cards[i].PositionInDeck;
                deck.Cards[i].PositionInDeck = temp;
            }
        }
    }
}