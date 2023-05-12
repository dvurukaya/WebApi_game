using Microsoft.EntityFrameworkCore;
using RestAPI_TicTacToe.Configurations;
using RestAPI_TicTacToe.Models;
using System.Collections.Generic;
using System.Reflection;

namespace RestAPI_TicTacToe.Data
{
    public class GameContext : DbContext
    {       
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        } 

        public DbSet<Game> Games { get; set; }        
        public DbSet<Player> Players { get; set; }
        public DbSet<Move> Moves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameConfiguration).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
