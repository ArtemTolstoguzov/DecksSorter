using System.Collections.Generic;
using System.Threading.Tasks;
using DecksSorter.Models;

namespace DecksSorter.Repositories
{
    public interface IDeckRepository
    {
        public IEnumerable<string> GetAllDecks();
        public Task<Deck> GetDeck(string name);
        public Task<Deck> CreateDeck();
        public Task DeleteDeck(Deck deck);
        public Task ShuffleDeck(Deck deck);
    }
}