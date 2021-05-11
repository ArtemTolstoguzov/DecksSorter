using System;
using System.Collections.Generic;
using System.Linq;

namespace DecksSorter.DTO
{
    public class DeckDto
    {
        public string Name { get; set; }
        public  List<CardDto> Cards { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((DeckDto) obj);
        }

        private bool Equals(DeckDto other)
        {
            return Name == other.Name && Cards.SequenceEqual(other.Cards);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Cards);
        }
    }
}