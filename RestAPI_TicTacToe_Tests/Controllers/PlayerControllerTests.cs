using Microsoft.AspNetCore.Mvc;
using Moq;
using RestAPI_TicTacToe.Controllers;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Services;

namespace RestAPI_TicTacToe_Tests.Controllers
{
    public class PlayerControllerTests
    {
        private readonly Mock<IPlayerService> _playerServiceMock;
        private readonly PlayerController _controller;

        public PlayerControllerTests()
        {
            _playerServiceMock = new Mock<IPlayerService>();
            _controller = new PlayerController(_playerServiceMock.Object);
        }

        [Fact]
        public async Task ReturnAllExistingPlayersAsync()
        {
            //Arrange
            var players = new List<Player>
            {
                new Player(),
                new Player(),
                new Player()
            };
            _playerServiceMock.Setup(x => x.GetAllPlayersAsync()).ReturnsAsync(players);

            //Act
            var result = await _controller.GetAllPlayersAsync();

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedPlayers = Assert.IsType<List<Player>>(expectedResult.Value);
            Assert.Equal(3, returnedPlayers.Count());
        }

        [Fact]
        public async Task ReturnNotFoundPlayersAsync()
        {
            //Arrange
            var players = new List<Player>
            {
            };
            _playerServiceMock.Setup(x => x.GetAllPlayersAsync()).ReturnsAsync(players);

            //Act
            var result = await _controller.GetAllPlayersAsync();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ReturnExistingPlayerByIdAsync()
        {
            //Arrange
            var playerId = 2;
            var player = new Player { Id = playerId, Name = "Tom" };
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(playerId)).ReturnsAsync(player);

            //Act
            var result = await _controller.GetPlayerByIdAsync(playerId);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedPlayer = Assert.IsType<Player>(expectedResult.Value);
            Assert.Equal(player.Id, returnedPlayer.Id);
            Assert.Equal(player.Name, returnedPlayer.Name);
        }

        [Fact]
        public async Task ReturnNotExistingPlayerByIdAsync()
        {
            //Arrange
            var playerId = 2;
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(playerId)).ReturnsAsync(null as Player);

            //Act
            var result = await _controller.GetPlayerByIdAsync(playerId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ReturnCreatedPlayerAsync()
        {
            //Arrange
            var playerName = "Evgen";
            var player = new Player { Name = playerName };
            _playerServiceMock.Setup(x => x.CreatePlayerAsync(playerName)).ReturnsAsync(player);

            //Act
            var result = await _controller.CreatePlayerAsync(playerName);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedPlayer = Assert.IsType<Player>(expectedResult.Value);
            Assert.Equal(player.Id, returnedPlayer.Id);
            Assert.Equal(player.Name, returnedPlayer.Name);
        }

        [Fact]
        public async Task ReturnUpdatedPlayerAsync()
        {
            //Arrange
            var playerId = 2;
            var player = new Player { Id = playerId, Name = "Lola" };
            _playerServiceMock.Setup(x => x.UpdatePlayerAsync(player)).ReturnsAsync(player);

            //Act
            var result = await _controller.UpdatePlayerAsync(playerId,player);

            //Assert
            var expectedResult = Assert.IsType<OkObjectResult>(result);
            var returnedPlayer = Assert.IsType<Player>(expectedResult.Value);
            Assert.Equal(player.Id, returnedPlayer.Id);
            Assert.Equal(player.Name, returnedPlayer.Name);
        }

        [Fact]
        public async Task ReturnNotUpdatedPlayerWithWrongIdAsync()
        {
            //Arrange
            var playerId = 2;
            var requestedId = 4;
            var player = new Player { Id = playerId, Name = "Lola" };

            //Act
            var result = await _controller.UpdatePlayerAsync(requestedId, player);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ReturnDeletedPlayerByIdAsync()
        {
            //Arrange
            var playerId = 5;
            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(x => x.GetPlayerByIdAsync(playerId))
                                          .ReturnsAsync(new Player { Id = playerId });
            var controller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await controller.DeletePlayerAsync(playerId);

            //Assert
            Assert.IsType<NoContentResult>(result);
            playerServiceMock.Verify(x => x.DeletePlayerAsync(playerId), Times.Once);
        }
    }
}
