using System.Linq;
using DecksSorter.Models;

namespace DecksSorter.DTO
{
    public class DeckConverter: IConverter<Deck, DeckDto>
    {
        private readonly IConverter<Card, CardDto> cardConverter;

        public DeckConverter(IConverter<Card, CardDto> cardConverter)
        {
            this.cardConverter = cardConverter;
        }

        public DeckDto Convert(Deck source)
        {
            var cards = source.Cards.Select(cardConverter.Convert);
            return new DeckDto {Cards = cards, Name = source.Name};
        }
    }
}