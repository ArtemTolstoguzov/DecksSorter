using DecksSorter.Models;

namespace DecksSorter.Shufflers
{
    public interface IShuffler
    {
        public void Shuffle(Deck deck);
    }
}