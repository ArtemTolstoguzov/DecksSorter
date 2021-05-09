#nullable disable

namespace DecksSorter.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public int DeckId { get; set; }
        public int PositionInDeck { get; set; }
        public CardSuits Suit { get; set; }
        public CardValues Value { get; set; }

        public virtual Deck Deck { get; set; }
    }
}
