using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DecksSorter.DB;
using DecksSorter.Models;
using DecksSorter.Shufflers;
using Microsoft.EntityFrameworkCore;

namespace DecksSorter.Services
{
    public class DeckService
    {
        private readonly DecksContext dbContext;
        private readonly IShuffler shuffler;

        public DeckService(DecksContext dbContext, IShuffler shuffler)
        {
            this.dbContext = dbContext;
            this.shuffler = shuffler;
        }

        public IEnumerable<string> GetAllDecks()
        {
            return dbContext.Decks.Select(d => d.Name);
        }
        
        public async Task<Deck> GetDeck(string name)
        {
            var deck = await dbContext.Decks.FirstOrDefaultAsync(d => d.Name == name);
            await dbContext.Entry(deck).Collection("Cards").LoadAsync();
            deck.Cards.Sort((c1, c2) => c1.PositionInDeck - c2.PositionInDeck);
            return deck;
        }

        public async Task<Deck> CreateDeck()
        {
            var name = Guid.NewGuid().ToString();
            await dbContext.Decks.AddAsync(new Deck {Name = name});
            await dbContext.SaveChangesAsync();
            return await GetDeck(name);
        }

        public async Task DeleteDeck(Deck deck)
        {
            dbContext.Decks.Remove(deck);
            await dbContext.SaveChangesAsync();
        }

        public async Task ShuffleDeck(Deck deck)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            shuffler.Shuffle(deck);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}