using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestAPI_TicTacToe.Models;

namespace RestAPI_TicTacToe.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(k => k.Id);

            builder
                .Property(s => s.Status)
                .IsRequired();

            builder
                .Property(w => w.WinnerId)
                .IsRequired();

            builder
                .Property(c => c.CodeGame)
                .IsRequired();

            builder
                .Property(d => d.Date)
                .IsRequired();

            builder
                .HasMany(m => m.Moves)
                .WithOne()
                .HasForeignKey(k => k.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.FirstPlayer)
                .WithMany()
                .HasForeignKey(k => k.FirstPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.SecondPlayer)
                .WithMany()
                .HasForeignKey(k => k.SecondPlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
