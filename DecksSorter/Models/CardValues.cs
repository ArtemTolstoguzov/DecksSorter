using NpgsqlTypes;

namespace DecksSorter.Models
{
    public enum CardValues
    {
        [PgName("Ace")]
        Ace,
        [PgName("Two")]
        Two,
        [PgName("Three")]
        Three,
        [PgName("Four")]
        Four,
        [PgName("Five")]
        Five,
        [PgName("Six")]
        Six,
        [PgName("Seven")]
        Seven,
        [PgName("Eight")]
        Eight,
        [PgName("Nine")]
        Nine,
        [PgName("Ten")]
        Ten,
        [PgName("Jack")]
        Jack,
        [PgName("Queen")]
        Queen,
        [PgName("King")]
        King
    }
}