using DecksSorter.Models;

namespace DecksSorter.DTO
{
    public class CardDto
    {
        public CardSuits Suit { get; set; }
        public CardValues Value { get; set; }
    }
}