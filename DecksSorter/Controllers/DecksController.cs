using System.Collections.Generic;
using System.Threading.Tasks;
using DecksSorter.DTO;
using DecksSorter.Models;
using DecksSorter.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DecksSorter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DecksController : ControllerBase
    {
        private readonly IDeckRepository deckRepository;
        private readonly IConverter<Deck, DeckDto> dtoConverter;

        public DecksController(IDeckRepository deckRepository, IConverter<Deck, DeckDto> dtoConverter)
        {
            this.deckRepository = deckRepository;
            this.dtoConverter = dtoConverter;
        }

        [HttpGet]
        public IEnumerable<string> GetAllDecks()
        {
            return deckRepository.GetAllDecks();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<DeckDto>> GetDeck(string name)
        {
            try
            {
                var deck = await deckRepository.GetDeck(name);
                return dtoConverter.Convert(deck);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateDeck()
        {
            var newDeck = await deckRepository.CreateDeck();
            return CreatedAtAction(nameof(GetDeck), new {name = newDeck.Name}, dtoConverter.Convert(newDeck));
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteDeck(string name)
        {
            try
            {
                var deck = await deckRepository.GetDeck(name);
                await deckRepository.DeleteDeck(deck);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("shuffle/{name}")]
        public async Task<ActionResult> ShuffleDeck(string name)
        {
            try
            {
                var deck = await deckRepository.GetDeck(name);
                await deckRepository.ShuffleDeck(deck);
                var shuffleDeck = await deckRepository.GetDeck(name);
                return AcceptedAtAction(nameof(GetDeck), new {name = shuffleDeck.Name},
                    dtoConverter.Convert(shuffleDeck));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}