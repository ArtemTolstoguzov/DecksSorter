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
        private readonly IDecksRepository decksRepository;
        private readonly IConverter<Deck, DeckDto> dtoConverter;

        public DecksController(IDecksRepository decksRepository, IConverter<Deck, DeckDto> dtoConverter)
        {
            this.decksRepository = decksRepository;
            this.dtoConverter = dtoConverter;
        }

        [HttpGet]
        public IEnumerable<string> GetAllDecks()
        {
            return decksRepository.GetAllDecks();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<DeckDto>> GetDeck(string name)
        {
            try
            {
                var deck = await decksRepository.GetDeck(name);
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
            var newDeck = await decksRepository.CreateDeck();
            return CreatedAtAction(nameof(GetDeck), new {name = newDeck.Name}, dtoConverter.Convert(newDeck));
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteDeck(string name)
        {
            try
            {
                var deck = await decksRepository.GetDeck(name);
                await decksRepository.DeleteDeck(deck);
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
                var deck = await decksRepository.GetDeck(name);
                await decksRepository.ShuffleDeck(deck);
                var shuffleDeck = await decksRepository.GetDeck(name);
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