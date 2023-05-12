using Microsoft.EntityFrameworkCore;
using RestAPI_TicTacToe.Data;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Repositories;

namespace RestAPI_TicTacToe_Tests.Repositories
{
    public class PlayerRepositoryTests
    {
        private readonly DbContextOptions<GameContext> _options;
        private readonly PlayerRepository _repository;

        public PlayerRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _repository = new PlayerRepository(new GameContext(_options));
        }
      

        [Fact]
        public async Task ReturnPlayerByIdAsync()
        {
            //Arrange
            var player = new Player { Id = 1, Name = "Somebody" };
            using (var context = new GameContext(_options))
            {
                await context.Players.AddAsync(player);
                await context.SaveChangesAsync();
            }

            //Act
            var result = await _repository.GetPlayerByIdAsync(player.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(player.Id, result.Id);
            Assert.Equal(player.Name, result.Name);
        }

        [Fact]
        public async Task ReturnAllPLayers()
        {
            //Arrange
            var players = new List<Player>
            {
                new Player {Id = 1, Name = "Somebody"},
                new Player {Id = 2, Name = "Name"}
            };

            using (var context = new GameContext(_options))
            {
                await context.Players.AddRangeAsync(players);
                await context.SaveChangesAsync();
            }

            //Act
            var result = await _repository.GetAllPlayersAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(players.Count, result.Count);
            Assert.Equal(players[0].Id, result[0].Id);
            Assert.Equal(players[0].Name, result[0].Name);
            Assert.Equal(players[1].Id, result[1].Id);
            Assert.Equal(players[1].Name, result[1].Name);
        }

        [Fact]
        public async Task ReturnPlayerByName()
        {
            //Arrange
            var player = new Player { Id = 1, Name = "Name" };
            using (var context = new GameContext(_options))
            {
                await context.Players.AddRangeAsync(player);
                await context.SaveChangesAsync();
            }

            //Act
            var result = await _repository.GetPlayerByNameAsync(player.Name);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(player.Id, result.Id);
            Assert.Equal(player.Name, result.Name);
        }

        [Fact]
        public async Task CreatePlayer()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: "CreatePlayer")
                .Options;
            using var dbContext = new GameContext(options);
            var playerRepository = new PlayerRepository(dbContext);

            var playerName = "Someone's Name";

            //Act
            var newPlayer = await playerRepository.CreatePlayerAsync(
                new Player { Name = playerName });
            var playerInDB = await playerRepository.GetPlayerByIdAsync(newPlayer.Id);

            //Assert
            Assert.NotNull(newPlayer);
            Assert.NotEqual(default(int), newPlayer.Id);
            Assert.Equal(playerName, newPlayer.Name);
            Assert.Equal(newPlayer, playerInDB);
        }

        [Fact]
        public async Task UpdatePlayer() 
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: "UpdatePlayer")
                .Options;
            using (var context = new GameContext(options))
            {
                var player = new Player { Id = 3, Name = "Name" };
                await context.Players.AddAsync(player);
                await context.SaveChangesAsync();
            }

            //Act
            using (var context = new GameContext(options))
            {
                var repository = new PlayerRepository(context);
                var updatePlayer = await repository.GetPlayerByNameAsync("Name");
                updatePlayer.Name = "New Name";
                await repository.UpdatePlayerAsync(updatePlayer);
            }

            //Assert
            using (var context = new GameContext(options))
            {
                var player = await context.Players.SingleAsync();
                Assert.Equal("New Name", player.Name);
            }
        }

        [Fact]
        public async Task DeletePlayer() 
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: "DeletePlayer")
                .Options;
            using (var context = new GameContext(options))
            {
                var player = new Player { Id = 6, Name = "Name" };
                await context.Players.AddAsync(player);
                await context.SaveChangesAsync();
            }

            //Act
            using (var context = new GameContext(options))
            {
                var repository = new PlayerRepository(context);
                var deletePlayer = await repository.GetPlayerByNameAsync("Name");
                await repository.DeletePlayerAsync(deletePlayer.Id);
            }

            //Assert
            using (var context = new GameContext(options))
            {
                var players = await context.Players.ToListAsync();
                Assert.Empty(players);
            }
        }
    }
}
