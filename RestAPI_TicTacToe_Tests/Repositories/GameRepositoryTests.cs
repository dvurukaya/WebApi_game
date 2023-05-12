using Microsoft.EntityFrameworkCore;
using RestAPI_TicTacToe.Data;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Repositories;
using RestAPI_TicTacToe.StaticInfo;

namespace RestAPI_TicTacToe_Tests.Repositories
{
    public class GameRepositoryTests
    {
        private DbContextOptions<GameContext> _options;
        private GameContext _dbContext;
        private GameRepository _gameRepository;

        public GameRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: "TicTacToe")
                .Options;
            _dbContext = new GameContext(_options);
            _gameRepository = new GameRepository(_dbContext);
        }

        [Fact]
        public async Task ReturnAllGamesAsync()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: "ReturnAllGamesAsync")
                .Options;
            using var dbContext = new GameContext(options);
            var gameRepository = new GameRepository(dbContext);

            await dbContext.Games.AddRangeAsync(
                new Game
                {
                    FirstPlayerId = 1,
                    SecondPlayerId = 2,
                    Status = GameStatus.GameOver,
                    Board = new int[] { }
                },
                new Game
                {
                    FirstPlayerId = 3,
                    SecondPlayerId = 4,
                    Status = GameStatus.GameOver,
                    Board = new int[] { }
                });
            await dbContext.SaveChangesAsync();

            //Act
            var games = await gameRepository.GetAllGamesAsync();

            //Assert
            Assert.Equal(2, games.Count());
        }

        [Fact]
        public async Task ReturnGameByIdAsync()
        {
            //Arrange
            var game = new Game
            {
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = GameStatus.GameOver,
                Board = new int[] { }
            };

            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _gameRepository.GetGameByIdAsync(game.Id);

            //Assert
            Assert.Equal(game.Id, result.Id);
        }

        [Fact]
        public async Task ReturnGameByCodeAsync()
        {
            //Arrange
            var game = new Game
            {
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = GameStatus.GameOver,
                Board = new int[] { },
                CodeGame = Guid.NewGuid(),
            };
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _gameRepository.GetGameByCodeAsync(game.CodeGame);

            //Assert
            Assert.Equal(game.CodeGame, result.CodeGame);
        }

        [Fact]
        public async Task GetGamesByDateAsync()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: "ReturnGamesByDateAsync")
                .Options;
            using var dbContext = new GameContext(options);
            var gameRepository = new GameRepository(dbContext);

            await dbContext.Games.AddRangeAsync(
                new Game
                {
                    Id = 1,
                    FirstPlayerId = 1,
                    SecondPlayerId = 2,
                    Board = new int[] { 1, 2, 1, 1, 2, 2, 2, 1, 0 },
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2018, 8, 12),
                    Status = GameStatus.GameOver,
                    Draw = true
                },
                new Game
                {
                    Id = 2,
                    FirstPlayerId = 2,
                    SecondPlayerId = 3,
                    Board = new int[] { 0, 0, 1, 1, 2, 2, 0, 0, 0 },
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2016, 8, 12),
                    Status = GameStatus.FirstPlayerTurn
                },
                new Game
                {
                    Id = 3,
                    FirstPlayerId = 4,
                    SecondPlayerId = 5,
                    Board = new int[] { 1, 2, 1, 2, 0, 1, 0, 0, 0 },
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2016,8,12),
                    Status = GameStatus.GameOver,
                    WinnerId = 4
                });
            await dbContext.SaveChangesAsync();
            var date = new DateTime(2016, 8, 12);

            //Act
            var result = await gameRepository.GetGamesByDateAsync(date);

            //Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateGameWithFirstPlayer()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: "CreateGameWithFirstPlayer")
                .Options;
            using var DbContext = new GameContext(options);
            var gameRepository = new GameRepository(DbContext);

            //Act
            var game = new Game
            {
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = GameStatus.Registered,
                Board = new int[] { },
                CodeGame = Guid.NewGuid(),
                Date = DateTime.Now
            };
            var result = await gameRepository.CreateGameWithFirstPlayerAsync(game);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(game.FirstPlayerId, result.FirstPlayerId);
            Assert.Equal(game.SecondPlayerId, result.SecondPlayerId);
            Assert.Equal(game.Date, result.Date);
            Assert.Equal(game.CodeGame, result.CodeGame);
            Assert.Equal(game.Status, result.Status);
        }

        [Fact]
        public async Task EntryGameWithSecondPlayer()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: "EntryGameWithSecondPlayer")
                .Options;
            using var DbContext = new GameContext(options);
            var gameRepository = new GameRepository(DbContext);

            var game = new Game
            {
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = GameStatus.Registered,
                Board = new int[] { },
                CodeGame = Guid.NewGuid(),
                Date = DateTime.Now
            };
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            game.Status = GameStatus.Started;

            //Act
            var result = await _gameRepository.EntryGameWithSecondPlayerAsync(game.CodeGame, game.Status);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(game.CodeGame, result.CodeGame);
            Assert.Equal(game.Status, result.Status);
        }

        [Fact]
        public async Task UpdateGame()
        {
            //Arrange
            var game = new Game
            {
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = GameStatus.Started,
                Board = new int[] { },
                CodeGame = Guid.NewGuid(),
                Date = DateTime.Now
            };
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();
            game.Status = GameStatus.FirstPlayerTurn;

            //Act
            var result = await _gameRepository.UpdateGameAsync(game);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(game.Id, result.Id);
            Assert.Equal(game.Status, result.Status);
        }

        [Fact]
        public async Task DeleteGame()
        {
            //Arrange
            var game = new Game
            {
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = GameStatus.Started,
                Board = new int[] { },
                CodeGame = Guid.NewGuid(),
                Date = DateTime.Now
            };
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            //Act
            await _gameRepository.DeleteGameByIdAsync(game.Id);

            //Assert
            var result = await _gameRepository.GetGameByIdAsync(game.Id);
            Assert.Null(result);
        }

    }
}
