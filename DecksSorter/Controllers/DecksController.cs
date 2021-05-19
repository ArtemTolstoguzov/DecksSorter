using System.Collections.Generic;
using System.Threading.Tasks;
using DecksSorter.DTO;
using DecksSorter.Models;
using DecksSorter.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DecksSorter.Controllers
{
    [Produces("application/json")]
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

        /// <summary>
        /// Returns the names of all Decks
        /// </summary>
        /// <returns>List of Deck names</returns>
        /// <response code="200">Returns a list of Deck names</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IEnumerable<string> GetAllDecks()
        {
            return decksRepository.GetAllDecks();
        }

        /// <summary>
        /// Returns a Deck
        /// </summary>
        /// <param name="name"></param>   
        /// <returns>A Deck</returns>
        /// <response code="200">Returns the Deck</response>
        /// <response code="400">If Deck does not exist</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Creates a Deck
        /// </summary>
        /// <returns>A newly created Deck</returns>
        /// <response code="201">Returns the newly created Deck</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult> CreateDeck()
        {
            var newDeck = await decksRepository.CreateDeck();
            return CreatedAtAction(nameof(GetDeck), new {name = newDeck.Name}, dtoConverter.Convert(newDeck));
        }

        /// <summary>
        /// Deletes a Deck
        /// </summary>
        /// <param name="name"></param>   
        /// <response code="204">Successful Deck deletion</response>
        /// <response code="400">If Deck does not exist</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Shuffles a Deck
        /// </summary>
        /// <param name="name"></param>   
        /// <returns>A shuffled Deck</returns>
        /// <response code="202">Returns the shuffled Deck</response>
        /// <response code="400">If Deck does not exist</response>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("shuffle/{name}")]
        public async Task<ActionResult> ShuffleDeck(string name)
        {
            try
            {
                var deck = await decksRepository.GetDeck(name);
                await decksRepository.ShuffleDeck(deck);
                var shuffledDeck = await decksRepository.GetDeck(name);
                return AcceptedAtAction(nameof(GetDeck), new {name = shuffledDeck.Name},
                    dtoConverter.Convert(shuffledDeck));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}