using System.Collections.Generic;
using System.IO;
using System.Linq;
using DecksSorter.Models;
using DecksSorter.Shufflers;
using Newtonsoft.Json;
using Xunit;

namespace Tests
{
    public class ShufflersTests
    {
        private const string OrderedDeckName = "b24d0ca6-c4c4-49f6-b6c2-8762fd6d7d72";

        [Fact]
        public static void CheckHandleShuffler()
        {
            var shuffler = new HandShuffler();
            var deck = GetOrderedDeck();

            shuffler.Shuffle(deck);
            deck.Cards.Sort((c1, c2) => c1.PositionInDeck - c2.PositionInDeck);

            var preprevious = deck.Cards[0];
            var previous = deck.Cards[1];
            var neighborsCount = 0;
            for (var i = 2; i < deck.Cards.Count; i++)
            {
                var current = deck.Cards[i];
                if (IsNeighbors(preprevious, previous) && IsNeighbors(previous, current))
                    neighborsCount++;
                preprevious = previous;
                previous = current;
            }
            Assert.True(neighborsCount < deck.Cards.Count * 0.1);
        }

        [Fact]
        public static void CheckSimpleShuffler()
        {
            var shuffler = new SimpleShuffler();
            var deck = GetOrderedDeck();

            shuffler.Shuffle(deck);
            deck.Cards.Sort((c1, c2) => c1.PositionInDeck - c2.PositionInDeck);

            var preprevious = deck.Cards[0];
            var previous = deck.Cards[1];
            var neighborsCount = 0;
            for (var i = 2; i < deck.Cards.Count; i++)
            {
                var current = deck.Cards[i];
                if (IsNeighbors(preprevious, previous) && IsNeighbors(previous, current))
                    neighborsCount++;
                preprevious = previous;
                previous = current;
            }
            Assert.True(neighborsCount < deck.Cards.Count * 0.05);
        }

        private static bool IsNeighbors(Card previous, Card current)
        {
            return previous.Value == current.Value && previous.Suit == current.Suit - 1 ||
                   previous.Value == CardValues.King && previous.Suit == CardSuits.Spades &&
                   current.Value == CardValues.Ace&& current.Suit == CardSuits.Clubs;
        }

        private static Deck GetOrderedDeck()
        {
            using var reader = new StreamReader("TestDecks.txt");
            var json = reader.ReadToEnd();
            var testDecks = JsonConvert.DeserializeObject<List<Deck>>(json).ToList();
            return testDecks.First(deck => deck.Name == OrderedDeckName);
        }
    }
}