using Moq;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Repositories;
using RestAPI_TicTacToe.Services;
using RestAPI_TicTacToe.StaticInfo;

namespace RestAPI_TicTacToe_Tests.Services
{
    public class GameServiceTests
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly Mock<IMoveRepository> _moveRepositoryMock;
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            _gameRepositoryMock = new Mock<IGameRepository>(); 
            _moveRepositoryMock = new Mock<IMoveRepository>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();

            _gameService = new GameService(_gameRepositoryMock.Object, _moveRepositoryMock.Object,
                                           _playerRepositoryMock.Object);                                  
        }

        [Fact]
        public async Task ReturnAllGames()
        {
            //Arrange
            var games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    FirstPlayerId = 1,
                    SecondPlayerId = 2,
                    Board = new int[] {1,2,1,1,2,2,2,1,0},
                    CodeGame = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Status = GameStatus.GameOver,
                    Draw = true
                },
                new Game
                {
                    Id = 2,
                    FirstPlayerId = 2,
                    SecondPlayerId = 3,
                    Board = new int[] {0,0,1,1,2,2,0,0,0},
                    CodeGame = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Status = GameStatus.FirstPlayerTurn
                },
                new Game
                {
                    Id = 3,
                    FirstPlayerId = 4,
                    SecondPlayerId = 5,
                    Board = new int[] {1,2,1,2,0,1,0,0,0},
                    CodeGame = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Status = GameStatus.GameOver,
                    WinnerId = 4
                }
            };
            _gameRepositoryMock.Setup(x => x.GetAllGamesAsync()).ReturnsAsync(games);

            //Act
            var result = await _gameService.GetAllGamesAsync();

            //Assert
            Assert.Equal(games, result);
        }

        [Fact]
        public async Task ReturnGameById()
        {
            //Arrange
            var game = new Game
            {
                Id = 1,
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Board = new int[] { 1, 2, 1, 1, 2, 2, 2, 1, 0 },
                CodeGame = Guid.NewGuid(),
                Date = DateTime.Now,
                Status = GameStatus.GameOver,
                Draw = true
            };
            _gameRepositoryMock.Setup(x => x.GetGameByIdAsync(game.Id)).ReturnsAsync(game);

            //Act
            var result = await _gameService.GetGameByIdAsync(game.Id);

            //Assert
            Assert.Equal(game, result);
        }

        [Fact]
        public async Task ReturnGameByCode()
        {
            //Arrange
            var game = new Game
            {
                Id = 2,
                FirstPlayerId = 2,
                SecondPlayerId = 3,
                Board = new int[] { 0, 0, 1, 1, 2, 2, 0, 0, 0 },
                CodeGame = Guid.NewGuid(),
                Date = DateTime.Now,
                Status = GameStatus.FirstPlayerTurn
            };
            _gameRepositoryMock.Setup(x => x.GetGameByCodeAsync(game.CodeGame)).ReturnsAsync(game);

            //Act
            var result = await _gameService.GetGameByCodeAsync(game.CodeGame);

            //Assert
            Assert.Equal(game, result);
        }

        [Fact]
        public async Task ReturnGamesByDates()
        {
            //Arrange
            var data = new DateTime(2022, 12, 12);
            var games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    FirstPlayerId = 1,
                    SecondPlayerId = 2,
                    Board = new int[] {1,2,1,1,2,2,2,1,0},
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2021, 10, 12),
                    Status = GameStatus.GameOver,
                    Draw = true
                },
                new Game
                {
                    Id = 2,
                    FirstPlayerId = 2,
                    SecondPlayerId = 3,
                    Board = new int[] {0,0,1,1,2,2,0,0,0},
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2022, 12, 12),
                    Status = GameStatus.FirstPlayerTurn
                },
                new Game
                {
                    Id = 3,
                    FirstPlayerId = 4,
                    SecondPlayerId = 5,
                    Board = new int[] {1,2,1,2,0,1,0,0,0},
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2022, 12, 12),
                    Status = GameStatus.GameOver,
                    WinnerId = 4
                }
            };
            _gameRepositoryMock.Setup(x => x.GetGamesByDateAsync(data)).ReturnsAsync(games);

            //Act
            var result = await _gameService.GetGamesByDateAsync(data);

            //Assert
            Assert.Equal(games.Count, result.Count);
        }

        [Fact]
        public async Task ReturnGamesByPlayersNames()
        {
            //Arrange
            var firstPlayerId = 1;
            var secondPlayerId = 2;
            var players = new List<Player>
            {
                new Player {Id = firstPlayerId, Name = "Tom"},
                new Player {Id = secondPlayerId, Name = "Andre"}
            };
            var games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    FirstPlayerId = firstPlayerId,
                    SecondPlayerId = secondPlayerId,
                    Board = new int[] {1,2,1,1,2,2,2,1,0},
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2022, 10, 12),
                    Status = GameStatus.GameOver,
                    Draw = true
                },
                new Game
                {
                    Id = 2,
                    FirstPlayerId = firstPlayerId,
                    SecondPlayerId = secondPlayerId,
                    Board = new int[] {0,0,1,1,2,2,0,0,0},
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2022, 12, 12),
                    Status = GameStatus.FirstPlayerTurn
                },
                new Game
                {
                    Id = 3,
                    FirstPlayerId = 4,
                    SecondPlayerId = 5,
                    Board = new int[] {1,2,1,2,0,1,0,0,0},
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2022, 12, 12),
                    Status = GameStatus.GameOver,
                    WinnerId = 4
                }
            };
            _gameRepositoryMock.Setup(x => x.GetAllGamesAsync()).ReturnsAsync(games);
            _playerRepositoryMock.Setup(x => x.GetPlayerByNameAsync(players[0].Name))
                                              .ReturnsAsync(players[0]);
            _playerRepositoryMock.Setup(x => x.GetPlayerByNameAsync(players[1].Name))
                                              .ReturnsAsync(players[1]);
            var expectedGamesCount = 2;

            //Act
            var result = await _gameService.GetGamesByPlayersNames(players[0].Name, players[1].Name);

            //Assert
            Assert.Equal(expectedGamesCount, result.Count);
        }

        [Fact]
        public async Task ReturnCreatedGameWithFirstPlayerAsync()
        {
            //Arrange
            var firstPlayerId = 4;
            var secondPlayerId = 5;
            var game = new Game
            {
                Id = 10,
                Board = new int[] { },
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Status = GameStatus.SecondPlayerTurn,
                CodeGame = Guid.NewGuid(),
                Date = new DateTime()
            };
            _gameRepositoryMock.Setup(x => x.CreateGameWithFirstPlayerAsync(It.IsAny<Game>()))
                                            .ReturnsAsync(game);

            //Act
            var result = await _gameService.CreateGameAsync(firstPlayerId,secondPlayerId);

            //Assert
            _gameRepositoryMock.Verify(x => x.CreateGameWithFirstPlayerAsync(It.Is<Game>(g =>
                                            g.FirstPlayerId == firstPlayerId &&
                                            g.SecondPlayerId == secondPlayerId)), Times.Once);
            Assert.Equal(game, result);
        }

        [Fact]
        public async Task ReturnNewStatus_EntryGameWithSecondPlayer()
        {
            //Arrange
            var firstPlayerId = 4;
            var secondPlayerId = 5;
            var Code = Guid.NewGuid();
            var game = new Game
            {
                Id = 10,
                Board = new int[] { },
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Status = GameStatus.SecondPlayerTurn,
                CodeGame = Code,
                Date = new DateTime()
            };
            var newStatus = GameStatus.Started;

            _gameRepositoryMock.Setup(x => x.GetGameByCodeAsync(game.CodeGame)).ReturnsAsync(game);
            _gameRepositoryMock.Setup(x => x.EntryGameWithSecondPlayerAsync(game.CodeGame, newStatus))
                                            .ReturnsAsync(game);
            
            //Act
            var result = await _gameService.EntryGameWithSecondPlayerAsync(Code);

            //Assert
            Assert.Equal(game.CodeGame, result.CodeGame);
            Assert.Equal(game.Status, result.Status);

        }

        [Fact]
        public async Task CreateExistingMoveAsync()
        {
            //Arrange
            var gameId = 3;
            var playerId = 2;
            var cell = 3;

            var move = new Move
            {
                GameId = gameId,
                PlayerId = playerId,
                Cell = cell,
                Element = Elements.X
            };

            var moves = new List<Move> { move };
            _moveRepositoryMock.Setup(x => x.GetAllMovesByGameIdAsync(gameId))
                                            .ReturnsAsync(moves);

            //Act
            //Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _gameService.CreateMoveAsync(gameId, playerId, cell));
        }

        [Fact]
        public async Task CreateMoveWithWrongGameIdAsync()
        {
            //Arrange
            var gameId = 3;
            _gameRepositoryMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync((Game)null);

            //Act
            //Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _gameService.CreateMoveAsync(gameId, 2, 1));
        }

        public async Task CreateMoveWithWinningCombinationFirstPlayerIdAsync()
        {
            //Arrange
            var gameId = 2;
            var firstPlayerId = 4;
            var secondPlayerId = 5;
            var firstCell = 0;
            var secondCell = 2;
            var thirdCell = 3;
            var fourthCell = 5;
            var fifthCell = 6;

            var gameMoves = new List<Move>
            {
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 0, Element = Elements.X },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = 2, Element = Elements.O },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 3, Element = Elements.X  },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = 5, Element = Elements.O}
            };

            _moveRepositoryMock.Setup(x => x.GetAllMovesByGameIdAsync(gameId)).ReturnsAsync(gameMoves);
            var game = new Game { Id = gameId, FirstPlayerId = firstPlayerId, SecondPlayerId = secondPlayerId, Board = new int[] { 1, 0, 2, 1, 0, 2, 1, 0, 0 }, 
                                                                 Status = GameStatus.FirstPlayerTurn };
            _gameRepositoryMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync(game);

            //Act
            var result = await _gameService.CreateMoveAsync(firstPlayerId, gameId, fifthCell);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(firstPlayerId, result.WinnerId);
            Assert.Equal(GameStatus.GameOver, result.Status);
        }

        public async Task CreateMoveWithWinningCombinationSecondPlayerIdAsync()
        {
            //Arrange
            var gameId = 5;
            var firstPlayerId = 10;
            var secondPlayerId = 11;
            var firstCell = 4;
            var secondCell = 0;
            var thirdCell = 7;
            var fourthCell = 1;
            var fifthCell = 3;
            var sixthCell = 2;

            var gameMoves = new List<Move>
            {
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 4, Element = Elements.X },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = 0, Element = Elements.O },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 7, Element = Elements.X  },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = 1, Element = Elements.O},
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 3, Element = Elements.X}
            };
        

            _moveRepositoryMock.Setup(x => x.GetAllMovesByGameIdAsync(gameId)).ReturnsAsync(gameMoves);
            var game = new Game
            {
                Id = gameId,
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Board = new int[] { 2, 2, 2, 1, 1, 0, 0, 1, 0 },
                Status = GameStatus.SecondPlayerTurn
            };
            _gameRepositoryMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync(game);

            //Act
            var result = await _gameService.CreateMoveAsync(secondPlayerId, gameId, fifthCell);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(secondPlayerId, result.WinnerId);
            Assert.Equal(GameStatus.GameOver, result.Status);
        }

        public async Task CreateMoveDrawIdAsync()
        {
            //Arrange
            var gameId = 2;
            var firstPlayerId = 4;
            var secondPlayerId = 5;
            var firstCell = 0;
            var secondCell = 4;
            var thirdCell = 2;
            var fourthCell = 1;
            var fifthCell = 7;
            var sixthCell = 5;
            var seventhCell = 3;
            var eigthCell = 6;

            var gameMoves = new List<Move>
            {
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 0, Element = Elements.X },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = 4, Element = Elements.O },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 2, Element = Elements.X },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = 1, Element = Elements.O },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 7, Element = Elements.X },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = 5, Element = Elements.O },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = 3, Element = Elements.X }
            };

            _moveRepositoryMock.Setup(x => x.GetAllMovesByGameIdAsync(gameId)).ReturnsAsync(gameMoves);
            var game = new Game
            {
                Id = gameId,
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Board = new int[] { 1, 2, 1, 1, 2, 2, 0, 1, 0 },
                Status = GameStatus.SecondPlayerTurn
            };
            _gameRepositoryMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync(game);

            //Act
            var result = await _gameService.CreateMoveAsync(secondPlayerId, gameId, eigthCell);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(GameStatus.GameOver, result.Status);
            Assert.Equal(game.WinnerId, result.WinnerId);
            Assert.True(result.Draw);
        }

        [Fact]
        public async Task DeleteGameAsync()
        {
            //Arrange
            var game = new Game
            {
                Id = 5,
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Board = new int[] { 1, 2, 1, 1, 2, 2, 2, 1, 0 },
                CodeGame = Guid.NewGuid(),
                Date = DateTime.Now,
                Status = GameStatus.GameOver,
                Draw = true
            };
            _gameRepositoryMock.Setup(x => x.DeleteGameByIdAsync(game.Id)).Returns(Task.CompletedTask);

            //Act
            await _gameService.DeleteGameAsync(game.Id);

            //Assert
            _gameRepositoryMock.Verify(x => x.DeleteGameByIdAsync(game.Id), Times.Once());
        }
    }
}
