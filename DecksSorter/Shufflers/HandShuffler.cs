using System;
using DecksSorter.Models;

namespace DecksSorter.Shufflers
{
    public class HandShuffler : IShuffler
    {
        private readonly Random rnd = new();
        
        public void Shuffle(Deck deck)
        {
            var shuffleCount = rnd.Next(10, 15);
            for (var _ = 0; _ < shuffleCount; _++)
            {
                var deviation = deck.Cards.Count / 10;
                var left = deck.Cards.Count / 2 + rnd.Next(-deviation, deviation);
                var right = deck.Cards.Count;
                var pos = 0;
                while (right > 2)
                {
                    for (var i = left; i < right; i++)
                    {
                        deck.Cards[i].PositionInDeck = pos;
                        pos++;
                    }

                    deviation = left / 10;
                    right = left;
                    left = left / 2 + rnd.Next(-deviation, deviation);
                }

                for (var i = 0; i < right; i++)
                {
                    deck.Cards[i].PositionInDeck = pos;
                    pos++;
                }

                deck.Cards.Sort((c1, c2) => c1.PositionInDeck - c2.PositionInDeck);
            }
        }
    }
}