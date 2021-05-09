using DecksSorter.Models;

namespace DecksSorter.DTO
{
    public class CardConverter : IConverter<Card, CardDto>
    {
        public CardDto Convert(Card source)
        {
            return new() {Suit = source.Suit, Value = source.Value};
        }
    }
}