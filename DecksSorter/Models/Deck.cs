using System.Collections.Generic;

#nullable disable

namespace DecksSorter.Models
{
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>();
        }

        public int DeckId { get; set; }
        public string Name { get; set; }

        public List<Card> Cards { get; set; }
    }
}
