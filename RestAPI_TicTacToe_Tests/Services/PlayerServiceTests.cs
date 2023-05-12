using Moq;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Repositories;
using RestAPI_TicTacToe.Services;

namespace RestAPI_TicTacToe_Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly PlayerService _playerService;

        public PlayerServiceTests()
        {
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _playerService = new PlayerService(_mockPlayerRepository.Object);
        }

        [Fact]
        public async Task GetPlayerByIdAsync() 
        {
            //Arrange
            int playerId = 3;
            var player = new Player { Id = playerId, Name = "John" };
            _mockPlayerRepository.Setup(x => x.GetPlayerByIdAsync(playerId))
                                              .ReturnsAsync(player);

            //Act
            var result = await _playerService.GetPlayerByIdAsync(playerId);

            //Assert
            Assert.Equal(playerId, result.Id);
        }

        [Fact]
        public async Task GetAllPlayersAsync() 
        {
            //Arrange
            var players = new List<Player>
            {
                new Player {Id = 1, Name = "First Name"},
                new Player {Id = 2, Name = "Second Name"}
            };
            _mockPlayerRepository.Setup(x => x.GetAllPlayersAsync())
                                 .ReturnsAsync(players);

            //Act
            var result = await _playerService.GetAllPlayersAsync();

            //Assert
            Assert.Equal(players, result);
        }

        [Fact]
        public async Task CreatePlayerAsync() 
        {
            //Arrange
            var playerName = "PlayerName";
            var player = new Player { Id = 1, Name = playerName };
            _mockPlayerRepository.Setup(x => x.CreatePlayerAsync(It.IsAny<Player>()))
                                 .ReturnsAsync(player);

            //Act
            var result = await _playerService.CreatePlayerAsync(playerName);

            //Assert
            Assert.Equal(player, result);

        }

        [Fact]
        public async Task UpdatePlayerAsync() 
        {
            //Arrange
            var player = new Player { Id = 2, Name = "Name" };
            _mockPlayerRepository.Setup(x => x.UpdatePlayerAsync(It.IsAny<Player>()))
                                 .ReturnsAsync(player);

            //Act
            var result = await _playerService.UpdatePlayerAsync(player);

            //Assert
            Assert.Equal(player, result);
        }

        [Fact]
        public async Task DeletePlayerAsync() 
        {
            //Arrange
            int playerId = 1;
            _mockPlayerRepository.Setup(x => x.DeletePlayerAsync(playerId))
                                 .Returns(Task.CompletedTask);

            //Act
            await _playerService.DeletePlayerAsync(playerId);

            //Assert
            _mockPlayerRepository.Verify(x => x.DeletePlayerAsync(playerId), Times.Once);
        }
    }
}
