using NpgsqlTypes;

namespace DecksSorter.Models
{
    public enum CardSuits
    {
        [PgName("Clubs")]
        Clubs,
        [PgName("Diamonds")]
        Diamonds,
        [PgName("Hearts")]
        Hearts,
        [PgName("Spades")]
        Spades
    }
}