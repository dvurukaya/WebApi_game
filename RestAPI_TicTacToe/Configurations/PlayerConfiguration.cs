using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestAPI_TicTacToe.Models;

namespace RestAPI_TicTacToe.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(k => k.Id);

            builder
                .Property(n => n.Name)
                .IsRequired();
        }
    }
}
