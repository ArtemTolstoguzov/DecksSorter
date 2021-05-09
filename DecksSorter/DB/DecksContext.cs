using DecksSorter.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

#nullable disable

namespace DecksSorter.DB
{
    public class DecksContext : DbContext
    {
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Deck> Decks { get; set; }
        
        public DecksContext()
        {
        }

        public DecksContext(DbContextOptions<DecksContext> options)
            : base(options)
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CardSuits>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CardValues>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum(null, "card_suits", new[] { "Clubs", "Diamonds", "Hearts", "Spades" })
                .HasPostgresEnum(null, "card_values", new[] { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" })
                .HasAnnotation("Relational:Collation", "ru_RU.UTF-8");

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("cards");

                entity.Property(e => e.CardId).HasColumnName("card_id");

                entity.Property(e => e.DeckId).HasColumnName("deck_id");

                entity.Property(e => e.PositionInDeck).HasColumnName("position_in_deck");
                
                entity.Property(e => e.Suit).HasColumnName("suit");
                
                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Deck)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.DeckId)
                    .HasConstraintName("cards_deck_id_fkey");
            });

            modelBuilder.Entity<Deck>(entity =>
            {
                entity.ToTable("decks");

                entity.HasIndex(e => e.Name, "decks_name_key")
                    .IsUnique();

                entity.Property(e => e.DeckId).HasColumnName("deck_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("name");
            });
        }
    }
}
