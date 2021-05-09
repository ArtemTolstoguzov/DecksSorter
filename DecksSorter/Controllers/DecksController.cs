using System.Collections.Generic;
using System.Threading.Tasks;
using DecksSorter.DTO;
using DecksSorter.Models;
using DecksSorter.Services;
using Microsoft.AspNetCore.Mvc;

namespace DecksSorter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DecksController : ControllerBase
    {
        private readonly DeckService deckService;
        private readonly IConverter<Deck, DeckDto> dtoConverter;

        public DecksController(DeckService deckService, IConverter<Deck, DeckDto> dtoConverter)
        {
            this.deckService = deckService;
            this.dtoConverter = dtoConverter;
        }

        [HttpGet]
        public IEnumerable<string> GetAllDecks()
        {
            return deckService.GetAllDecks();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<DeckDto>> GetDeck(string name)
        {
            try
            {
                var deck = await deckService.GetDeck(name);
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
            var newDeck = await deckService.CreateDeck();
            return CreatedAtAction(nameof(GetDeck), new {name = newDeck.Name}, dtoConverter.Convert(newDeck));
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteDeck(string name)
        {
            try
            {
                var deck = await deckService.GetDeck(name);
                await deckService.DeleteDeck(deck);
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
                var deck = await deckService.GetDeck(name);
                await deckService.ShuffleDeck(deck);
                var shuffleDeck = await deckService.GetDeck(name);
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