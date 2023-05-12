using Microsoft.AspNetCore.Mvc;
using Moq;
using RestAPI_TicTacToe.Controllers;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Services;
using RestAPI_TicTacToe.StaticInfo;

namespace RestAPI_TicTacToe_Tests.Controllers
{
    public class GameControllerTests 
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly GameController _controller;

        public GameControllerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _controller = new GameController(_gameServiceMock.Object); 
        }

        [Fact]
        public async Task ReturnAllExistingGamesAsync()
        {
            //Arrange
            var games = new List<Game>
            {
                new Game(),
                new Game(),
                new Game()
            };
            _gameServiceMock.Setup(x => x.GetAllGamesAsync()).ReturnsAsync(games);

            //Act
            var result = await _controller.GetAllGamesAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGames = Assert.IsType<List<Game>> (okResult.Value);
            Assert.Equal(3, returnedGames.Count());
        }

        [Fact]
        public async Task ReturnNotFoundGamesAsync()
        {
            //Arrange
            var games = new List<Game>
            {
            };
            _gameServiceMock.Setup(x => x.GetAllGamesAsync()).ReturnsAsync(games);

            //Act
            var result = await _controller.GetAllGamesAsync();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ReturnExistingGameByIdAsync()
        {
            //Arrange
            var gameId = 2;
            var game = new Game
            {
                Id = gameId,
                Board = new int[] { },
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = GameStatus.SecondPlayerTurn,
                CodeGame = Guid.NewGuid(),
                Date = new DateTime()
            };
            _gameServiceMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync(game);

            //Act
            var result = await _controller.GetGameByIdAsync(gameId);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedGame = Assert.IsType<Game>(expectedResult.Value);
            Assert.Equal(gameId, returnedGame.Id);
        }

        [Fact]
        public async Task ReturnNotExistingGameByIdAsync()
        {
            //Arrange
            var gameId = 2;
            _gameServiceMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync((Game)null);

            //Act
            var result = await _controller.GetGameByIdAsync(gameId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ReturnExisingGameByCodeAsync()
        {
            //Arrange
            var code = Guid.NewGuid();
            var game = new Game
            {
                Id = 1,
                Board = new int[] { },
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = GameStatus.SecondPlayerTurn,
                CodeGame = code,
                Date = new DateTime()
            };
            _gameServiceMock.Setup(x => x.GetGameByCodeAsync(code)).ReturnsAsync(game);

            //Act
            var result = await _controller.GetGameByCodeAsync(code);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedGames = Assert.IsType<Game>(expectedResult.Value);
            Assert.Equal(game, returnedGames);
        }

        [Fact]
        public async Task ReturnNotExisingGameByCodeAsync()
        {
            //Arrange
            var code = Guid.NewGuid();
            _gameServiceMock.Setup(x => x.GetGameByCodeAsync(code)).ReturnsAsync((Game)null);

            //Act
            var result = await _controller.GetGameByCodeAsync(code);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ReturnGamesByDateAsync()
        {
            //Arrange
            var data = new DateTime(2019, 12, 12);
            var games = new List<Game>
            {
                new Game
                {
                    Id = 5,
                    FirstPlayerId = 2,
                    SecondPlayerId = 3,
                    Board = new int[] {0,0,1,1,2,2,0,0,0},
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2019, 12, 12),
                    Status = GameStatus.FirstPlayerTurn
                },
                new Game
                {
                    Id = 3,
                    FirstPlayerId = 4,
                    SecondPlayerId = 5,
                    Board = new int[] {1,2,1,2,0,1,0,0,0},
                    CodeGame = Guid.NewGuid(),
                    Date = new DateTime(2019, 12, 12),
                    Status = GameStatus.GameOver,
                    WinnerId = 4
                }
            };
            _gameServiceMock.Setup(x => x.GetGamesByDateAsync(data)).ReturnsAsync(games);

            //Act
            var result = await _controller.GetGamesByDateAsync(data);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedGames = Assert.IsType<List<Game>>(expectedResult.Value);
            Assert.Equal(2, returnedGames.Count());
        }

        [Fact]
        public async Task ReturnGamesByNotExistingDateAsync()
        {
            var data = DateTime.Now;
            var games = new List<Game>
            {
            };
            _gameServiceMock.Setup(x => x.GetGamesByDateAsync(data)).ReturnsAsync(games);

            //Act
            var result = await _controller.GetGamesByDateAsync(data);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ReturnGamesByPlayersNamesAsync()
        {
            //Arrange
            int firstPlayerId = 1;
            int secondPlayerId = 2;
            var players = new List<Player>
            {
                new Player {Id = firstPlayerId, Name = "Ann"},
                new Player {Id = secondPlayerId, Name = "Greg"}
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
                }
            };
            _gameServiceMock.Setup(x => x.GetGamesByPlayersNames(players[0].Name, players[1].Name)).ReturnsAsync(games);

            //Act
            var result = await _controller.GetGamesByPlayersNamesAsync(players[0].Name, players[1].Name);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedGames = Assert.IsType<List<Game>>(expectedResult.Value);
            Assert.Equal(2, returnedGames.Count());
        }

        [Fact]
        public async Task ReturnNotExistingGamesByPlayersNamesAsync()
        {
            //Arrange
            int firstPlayerId = 1;
            int secondPlayerId = 2;
            var players = new List<Player>
            {
                new Player {Id = firstPlayerId, Name = "Oleg"},
                new Player {Id = secondPlayerId, Name = "Natasha"}
            };
            var games = new List<Game>
            {
            };
            
            _gameServiceMock.Setup(x => x.GetGamesByPlayersNames(players[0].Name, players[1].Name)).ReturnsAsync(games);

            //Act
            var result = await _controller.GetGamesByPlayersNamesAsync(players[0].Name, players[1].Name);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task ReturnCreatedGameAsync()
        {
            //Arrange
            int firstPlayerId = 1;
            int secondPlayerId = 2;
            var firstPlayer = new Player { Id = firstPlayerId, Name = "Slava" };
            var secondPlayer = new Player { Id = secondPlayerId, Name = " " };
            var game = new Game
            {
                Id = 1,
                CodeGame = Guid.NewGuid(),
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Status = GameStatus.Registered
            };
            _gameServiceMock.Setup(x => x.CreateGameAsync(firstPlayerId, secondPlayerId)).ReturnsAsync(game);

            //Act
            var result = await _controller.CreateGameAsync(firstPlayerId, secondPlayerId);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedGame = Assert.IsType<Game>(expectedResult.Value);
            Assert.Equal(game, returnedGame);
        }

        [Fact]
        public async Task ReturnJoinedGameAsync()
        {
            var code = Guid.NewGuid();
            int firstPlayerId = 1;
            int secondPlayerId = 2;

            var players = new List<Player>
            {
                new Player { Id = firstPlayerId, Name = "Slava" },
                new Player { Id = secondPlayerId, Name = "Sveta" }
            };
            var game = new Game
            {
                Id = 1,
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Status = GameStatus.Registered,
                CodeGame = code
            };
            _gameServiceMock.Setup(x => x.GetGameByCodeAsync(code)).ReturnsAsync(game);
            _gameServiceMock.Setup(x => x.EntryGameWithSecondPlayerAsync(code)).ReturnsAsync(game);
            
            //Act
            var result = await _controller.UpdateGameToJoinAsync(code);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedGame = Assert.IsType<Game>(expectedResult.Value);
            Assert.Equal(game, returnedGame);
        }

        [Fact]
        public async Task ReturnJoinBadRequestGameAsync()
        {
            var code = Guid.NewGuid();
            int firstPlayerId = 1;
            int secondPlayerId = 2;

            var game = new Game
            {
                Id = 1,
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Status = GameStatus.Registered,
                CodeGame = Guid.NewGuid()
            };
            _gameServiceMock.Setup(x => x.GetGameByCodeAsync(code)).ReturnsAsync((Game)null);
            _gameServiceMock.Setup(x => x.EntryGameWithSecondPlayerAsync(code)).ReturnsAsync((Game)null);

            //Act
            var result = await _controller.UpdateGameToJoinAsync(code);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ReturnMoveIsCreatedAsync()
        {
            //Arrange
            int gameId = 2;
            int cell = 3;

            int firstPlayerId = 1;
            int secondPlayerId = 2;
            
            var players = new List<Player>
            {
                new Player { Id = firstPlayerId, Name = "Slava" },
                new Player { Id = secondPlayerId, Name = "Sveta" }
            };
            var game = new Game
            {
                Id = gameId,
                CodeGame = Guid.NewGuid(),
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Status = GameStatus.SecondPlayerTurn,
                Board = new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            _gameServiceMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync(game);
       
            //Act
            var result = await _controller.CreateMoveAsync(firstPlayerId, gameId, cell);
            
            //Assert
            _gameServiceMock.Verify(x => x.CreateMoveAsync(firstPlayerId, gameId, cell), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            
        }

        [Fact]
        public async Task ReturnMoveIsExistingAlreadyAsync()
        {
            //Arrange
            int gameId = 2;
            int cell = 4;
            int firstPlayerId = 1;
            
            var game = new Game
            {
                Id = gameId,
                CodeGame = Guid.NewGuid(),
                Status = GameStatus.FirstPlayerTurn,
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = ++firstPlayerId
            };
            string badMessage = $"Try again.The cell you requested [{cell}] is already used";


            _gameServiceMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync(game);
            _gameServiceMock.Setup(x => x.CreateMoveAsync(firstPlayerId, gameId, cell))
                                         .ThrowsAsync(new ApplicationException(badMessage));

            //Act
            var result = await _controller.CreateMoveAsync(firstPlayerId, gameId, cell);

            //Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badMessage, badResult.Value);
        }

        [Fact]
        public async Task ReturnMoveWhenGameIsOverAsync()
        {
            //Arrange
            int gameId = 3;
            int cell = 4;
            int firstPlayerId = 1;

            var player = new Player { Id = firstPlayerId, Name = "Slava" };
            var game = new Game
            {
                Id = gameId,
                Status = GameStatus.GameOver,
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = ++firstPlayerId,
            };
            string badMessage = $"Game {game} is already finished";

            _gameServiceMock.Setup(x => x.GetGameByIdAsync(gameId)).ReturnsAsync(game);
            
            //Act
            var result = await _controller.CreateMoveAsync(firstPlayerId, gameId, cell);

            //Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badMessage, badResult.Value);       
        }
    }
}
