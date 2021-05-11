using System;
using DecksSorter.Models;

namespace DecksSorter.DTO
{
    public class CardDto
    {
        public CardSuits Suit { get; set; }
        public CardValues Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((CardDto) obj);
        }

        private bool Equals(CardDto other)
        {
            return Suit == other.Suit && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Suit, (int) Value);
        }
    }
}