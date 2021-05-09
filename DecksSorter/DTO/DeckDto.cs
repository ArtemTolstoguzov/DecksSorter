using System.Collections.Generic;

namespace DecksSorter.DTO
{
    public class DeckDto
    {
        public string Name { get; set; }
        public  IEnumerable<CardDto> Cards { get; set; }
    }
}